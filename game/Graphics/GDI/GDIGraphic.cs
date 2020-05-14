using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Graphics.GDI;
using Graphics.Gizmos;

public class GDIGraphic : IGraphic
{
    private int width;
    private int height;

    private HandleRef handle;
    private BITMAPINFO info;
    private Matrix3x3 viewMatrix;
    private RendererQueue rendererQueue;

    private int[] frame;
    private int[] depth;
    private int[] clear;


    public void Draw()
    {
        GDIHelper.SetDIBitsToDevice(handle, 0, 0, width, height, 0, 0, 0, height, ref frame[0], ref info, 0);

        clear.CopyTo(frame, 0);
        clear.CopyTo(depth, 0);
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        viewMatrix.m20 = width * 0.5F - camera.gameObject.transform.position.X;
        viewMatrix.m21 = height * 0.5F + camera.gameObject.transform.position.Y;
     
        Parallel.For(0, gameObjects.Length, i =>
        {
            GameObject gameObject = gameObjects[i];
            gameObject.Update();

            Sprite sprite = gameObject.renderer.sprite;

            if (sprite != null && camera.viewport.Contain(gameObject.transform.position))
            {
                Vector delta = gameObject.transform.scale * 0.5f;

                Vector from = Matrix3x3.Multiply(viewMatrix, gameObject.transform.position - delta);
                Vector to = Matrix3x3.Multiply(viewMatrix, gameObject.transform.position + delta);

                int start_x = (int)from.X;
                int start_y = (int)from.Y;

                int end_x = (int)to.X;
                int end_y = (int)to.Y;

                float uv_to_width = sprite.Width / GameMath.Abs(start_x - end_x);
                float uv_to_height = sprite.Height / GameMath.Abs(start_y - end_y);

                for (int y = start_y; y < end_y; y++)
                {
                    for (int x = start_x; x < end_x; x++)
                    {
                        if (x >= 0 && x < width && y >= 0 && y < height)
                        {
                            int screen_idx = (x + y * width);
                            int uv_idx =
                                (int)((x - start_x) * uv_to_width) +
                                (int)((y - start_y) * uv_to_height) * sprite.Height;


                            if (depth[screen_idx] < gameObject.Layer)
                            {
                                if (uv_idx < sprite.Buffer.Length)
                                {
                                    frame[screen_idx] = sprite.Buffer[uv_idx];
                                    depth[screen_idx] = gameObject.Layer;
                                }
                            }
                        }
                    }
                }
            }
        });

        for (int index = 0; index < Gizmos.currentPrimitive; index++)
            Gizmos.primitives[index].Draw(viewMatrix, width, height, ref frame);

        Gizmos.currentPrimitive = 0;
    }

    public static GDIGraphic Create(int width, int height, HandleRef handle)
    {
        float size_unit = height * 0.08F;

        GDIGraphic graphic = new GDIGraphic();

        graphic.width = width;
        graphic.height = height;
        graphic.info = GDIHelper.CreateBitmapinfo(width, height);
        graphic.handle = handle;

        graphic.frame = new int[width * height];
        graphic.depth = new int[width * height];

        graphic.rendererQueue = new RendererQueue();
        graphic.viewMatrix = new Matrix3x3(
            size_unit, 0, width / 2,
            0, size_unit, height / 2,
            0, 0, 1);

        graphic.clear = new int[width * height];
       
        return graphic;
    }

    public void Release()
    {

    }}