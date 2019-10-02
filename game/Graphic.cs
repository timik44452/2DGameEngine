using System;
using System.Runtime.InteropServices;
using System.Threading;


public class Graphic
{
    public static Graphic graphics;

    private HandleRef handleRef;
    private GDIHelper.BITMAPINFO bitmapInfo;

    private int width;
    private int height;
    private int bufferSize;

    private int[] data;
    private int[] layersBuffer;

    public Graphic(int width, int height, HandleRef handleRef)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];
        bitmapInfo = GDIHelper.CreateBitmapinfo(width, height);

        this.handleRef = handleRef;
    }

    public void Draw()
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

    public unsafe void Blend(LightMap lightMap)
    {
        
    }

    internal unsafe void DrawGameObject(int delta, int layer, int width, int height, CIntegrations.Color[] sprite)
    {
        CIntegrations.draw_sprite(ref data[0], ref sprite[0], ref layersBuffer[0], delta, layer, this.width, this.height, width, height, bufferSize);
    }
}

