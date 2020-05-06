using System;
using System.Runtime.InteropServices;

using Graphics.GDI;

public class GDIGraphic : IGraphic
{
    private int width;
    private int height;

    private HandleRef handle;
    private BITMAPINFO info;
    private Matrix viewMatrix;
    private RendererQueue rendererQueue;

    private int[] frame;

    public void Draw()
    {
        GDIHelper.SetDIBitsToDevice(handle, 0, 0, width, height, 0, 0, 0, height, ref frame[0], ref info, 0);
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

                Vector from = Matrix.Multiply(viewMatrix, worldTransform - delta);
                Vector to = Matrix.Multiply(viewMatrix, worldTransform + delta);

                
            }
        }
    }

    public static GDIGraphic Create(int width, int height, HandleRef handle)
    {
        GDIGraphic graphic = new GDIGraphic();

        graphic.width = width;
        graphic.height = height;
        graphic.info = GDIHelper.CreateBitmapinfo(width, height);
        graphic.handle = handle;
        graphic.frame = new int[width * height];
        graphic.rendererQueue = new RendererQueue();
        graphic.viewMatrix = new Matrix(
            100F / width, 0,
            0, 100F / height);

        return graphic;
    }

    public void Release()
    {
        
    }
}