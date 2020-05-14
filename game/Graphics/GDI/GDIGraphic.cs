using Graphics.GDI;

using System.Runtime.InteropServices;

public class GDIGraphic : IGraphic
{
    [DllImport("GraphicProcessor")]
    private static extern void draw(float x, float y, float w, float h, ref int sprite, int sw, int sh);
    [DllImport("GraphicProcessor")]
    private static extern void init(ref int _frame, ref int depth, int _width, int _height);
    [DllImport("GraphicProcessor")]
    private static extern void clear();

    private int width;
    private int height;

    private HandleRef handle;
    private BITMAPINFO info;
    private Matrix3x3 viewMatrix;

    private int[] frame;
    private int[] depth;
    //private int[] clear;


    public void Draw()
    {
        GDIHelper.SetDIBitsToDevice(handle, 0, 0, width, height, 0, 0, 0, height, ref frame[0], ref info, 0);
        clear();
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        viewMatrix.m20 = width * 0.5F - camera.gameObject.transform.position.X;
        viewMatrix.m21 = height * 0.5F + camera.gameObject.transform.position.Y;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject gameObject = gameObjects[i];

            Sprite sprite = gameObject.renderer.sprite;

            if (sprite != null && camera.viewport.Contain(gameObject.transform.position))
            {
                Vector delta = gameObject.transform.scale * 0.5f;

                Vector from = Matrix3x3.Multiply(viewMatrix, gameObject.transform.position - delta);
                Vector to = Matrix3x3.Multiply(viewMatrix, gameObject.transform.position + delta);

                float x = (from.X + to.X) * 0.5F;
                float y = (from.Y + to.Y) * 0.5F;
                float w = GameMath.Abs(from.X - to.X);
                float h = GameMath.Abs(from.Y - to.Y);

                draw(x, y, w, h, ref sprite.Buffer[0], sprite.Width, sprite.Height);
            }
        }

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

        graphic.viewMatrix = new Matrix3x3(
            size_unit, 0, width / 2,
            0, size_unit, height / 2,
            0, 0, 1);

        init(ref graphic.frame[0], ref graphic.depth[0], width, height);

        return graphic;
    }

    public void Release()
    {

    }
}