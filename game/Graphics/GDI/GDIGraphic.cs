using System.Runtime.InteropServices;

using Graphics.GDI;

public class GDIGraphic : IGraphic
{
    private int width;
    private int height;
    
    private HandleRef handle;
    private BITMAPINFO info;
    private Matrix3x3 viewMatrix;
    private RendererQueue rendererQueue;

    private int[] frame;
    private int[] depthBuffer;
    private int[] depthBuffer_updateTrigger;
    private int updateTrigger;

    public void Draw()
    {
        GDIHelper.SetDIBitsToDevice(handle, 0, 0, width, height, 0, 0, 0, height, ref frame[0], ref info, 0);
        updateTrigger = (updateTrigger++) % 2;
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        rendererQueue.CheckVertexBuffer(gameObjects.Length * 6);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject gameObject = gameObjects[i];
            gameObject.Update();

            Sprite sprite = gameObject.renderer.sprite;

            if (sprite != null && camera.viewport.Contain(gameObject.transform.position))
            {
                Vector delta = gameObject.transform.scale * 0.5f;
                Vector worldTransform = gameObject.transform.position - camera.gameObject.transform.position;

                Vector from = Matrix3x3.Multiply(viewMatrix, worldTransform - delta);
                Vector to = Matrix3x3.Multiply(viewMatrix, worldTransform + delta);

                int start_x = (int)from.X;
                int start_y = (int)from.Y;

                int end_x = (int)to.X;
                int end_y = (int)to.Y;

                float uv_to_width = sprite.Width / GameMath.Abs(start_x - end_x);
                float uv_to_height = sprite.Height / GameMath.Abs(start_y - end_y);

                for (int y = start_y; y < end_y; y++)
                    for (int x = start_x; x < end_x; x++)
                    {
                        if (x >= 0 && x < width && y >= 0 && y < height)
                        {
                            int screen_idx = x + y * width;
                            int uv_idx = 
                                (int)((x - start_x) * uv_to_width) + 
                                (int)((y - start_y) * uv_to_height) * sprite.Height;


                            if (depthBuffer[screen_idx] < gameObject.Layer || depthBuffer_updateTrigger[screen_idx] != updateTrigger)
                            {
                                if (uv_idx < sprite.Buffer.Length)
                                {
                                    frame[screen_idx] = sprite.Buffer[uv_idx];

                                    depthBuffer[screen_idx] = gameObject.Layer;
                                    depthBuffer_updateTrigger[screen_idx] = updateTrigger;
                                }
                            }
                        }
                    }
            }
        }
    }

    public static GDIGraphic Create(int width, int height, HandleRef handle)
    {
        float size_unit = height * 0.1F;

        GDIGraphic graphic = new GDIGraphic();

        graphic.width = width;
        graphic.height = height;
        graphic.info = GDIHelper.CreateBitmapinfo(width, height);
        graphic.handle = handle;

        graphic.frame = new int[width * height];
        graphic.depthBuffer = new int[width * height];
        graphic.depthBuffer_updateTrigger = new int[width * height];

        graphic.rendererQueue = new RendererQueue();
        graphic.viewMatrix = new Matrix3x3(
            size_unit, 0, width / 2,
            0, size_unit, height / 2,
            0, 0, 1);

        return graphic;
    }

    public void Release()
    {
        
    }
}