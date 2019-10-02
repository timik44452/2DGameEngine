using System;
using System.Runtime.InteropServices;
using System.Drawing;


public class Graphic
{
    public static Graphic graphics { get; private set; }

    private HandleRef handleRef;
    private GDIHelper.BITMAPINFO bitmapInfo;

    private int width;
    private int height;
    private int bufferSize;

    private int[] data;
    private int[] layersBuffer;

    private Graphic(int width, int height, Graphics context)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];
        bitmapInfo = GDIHelper.CreateBitmapinfo(width, height);

        handleRef = new HandleRef(context, context.GetHdc());

        graphics = this;
    }

    public static Graphic Create(int width, int height, Graphics context)
    {
        return new Graphic(width, height, context);
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        DateTime now = DateTime.Now;

        //for (int i = 0; i < gameObjects.Length; i++)
         //   DrawGameObject(camera, gameObjects[i]);

        System.Threading.Tasks.Parallel.For(0, gameObjects.Length, i => DrawGameObject(camera, gameObjects[i]));

        Time.deltaTime = (float)(DateTime.Now - now).TotalMilliseconds;
    }

    public void GDIDraw()
    {
        CIntegrations.SetDIBitsToDevice(handleRef, 0, 0, width, height, 0, 0, 0, height, ref data[0], ref bitmapInfo, 0);
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (color.A == 1)
        {
            int index = x + y * width;

            if (index >= 0 && index < bufferSize)
            {
                data[index] = color.rgb;
            }
        }
    }

    public void SetPixel(int index, short layer, Color color)
    {
        if (index >= 0 && index < bufferSize && layer > layersBuffer[index])
        {
            layersBuffer[index] = layer;
            data[index] = color.rgb;
        }
    }

    public void Clear()
    {
        CIntegrations.clear(ref data[0], 0, bufferSize);
        CIntegrations.clear(ref layersBuffer[0], int.MinValue, bufferSize);
    }

    public void DrawGameObject(int delta, int layer, int width, int height, CIntegrations.Color[] sprite)
    {
        CIntegrations.draw_sprite(ref data[0], ref sprite[0], ref layersBuffer[0], delta, layer, this.width, this.height, width, height, bufferSize);
    }

    private void DrawGameObject(Camera camera, GameObject gameObject)
    {
        int posX = (int)((gameObject.transform.position.X - camera.position.X) * 32);
        int posY = (int)((gameObject.transform.position.Y - camera.position.Y) * 32);

        gameObject.Update();

        int delta = posX + posY * width;

        Sprite sprite = gameObject.renderer.sprite;

        if (camera.viewport.Contain(posX, posY) && sprite != null)
        {
            graphics.DrawGameObject(delta,
                gameObject.Layer,
                sprite.Width * gameObject.transform.HorizontalOrientation,
                sprite.Height * gameObject.transform.VerticalOrientation,
                sprite.GetTColors());
        }
    }
}

