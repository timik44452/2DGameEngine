using Graphics.GDI;
using System;
using System.Runtime.InteropServices;

public class GDIGraphic : IGraphic
{
    [DllImport("GraphicProcessor", CallingConvention = CallingConvention.StdCall)]
    private static extern void draw(float x, float y, float w, float h, IntPtr spritePtr, int sw, int sh);
    [DllImport("GraphicProcessor")]
    private static extern void init(ref int _frame, ref int depth, int _width, int _height);
    [DllImport("GraphicProcessor")]
    private static extern void clear();

    private int width;
    private int height;

    private HandleRef handle;
    private BITMAPINFO info;

    private Matrix3x3 worldViewMatrix;
    private Matrix3x3 screenViewMatrix;

    private int[] frame;
    private int[] depth;


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

        graphic.worldViewMatrix = new Matrix3x3(
            size_unit, 0, width / 2,
            0, size_unit, height / 2,
            0, 0, 1);

        graphic.screenViewMatrix = new Matrix3x3(
            width, 0, width / 2,
            0, height, height / 2,
            0, 0, 1);

        init(ref graphic.frame[0], ref graphic.depth[0], width, height);

        return graphic;
    }


    public void BeginRenderer(Camera camera)
    {
        worldViewMatrix.m20 = width * 0.5F - camera.gameObject.transform.position.X;
        worldViewMatrix.m21 = height * 0.5F + camera.gameObject.transform.position.Y;
    }

    public void Draw(Vector position, Vector scale, Sprite sprite, bool useWorldSpace)
    {
        if (sprite != null)// && camera.viewport.Contain(gameObject.transform.position))
        {
            Vector delta = scale * 0.5f;
            Matrix3x3 currentMatrix = useWorldSpace ? worldViewMatrix : screenViewMatrix;

            Vector from = Matrix3x3.Multiply(currentMatrix, position - delta);
            Vector to = Matrix3x3.Multiply(currentMatrix, position + delta);

            float x = (from.X + to.X) * 0.5F;
            float y = (from.Y + to.Y) * 0.5F;

            float w = GameMath.Abs(from.X - to.X);
            float h = GameMath.Abs(from.Y - to.Y);

            draw(x, y, w, h, sprite.GetIntPtr(), sprite.Width, sprite.Height);
        }
    }

    public void EndRenderer()
    {
        for (int index = 0; index < Gizmos.currentPrimitive; index++)
            Gizmos.primitives[index].Draw(worldViewMatrix, width, height, ref frame);

        Gizmos.currentPrimitive = 0;

        GDIHelper.SetDIBitsToDevice(handle, 0, 0, width, height, 0, 0, 0, height, ref frame[0], ref info, 0);

        clear();
    }

    

    public void Release()
    {

    }
}