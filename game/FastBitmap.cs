using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


public class FastBitmap
{
    public readonly int width;
    public readonly int height;


    private GDIHelper.BITMAPINFO bitmapInfo;

    private int[] data;

    public FastBitmap(int Width, int Height)
    {
        width = Width;
        height = Height;

        data = new int[Width * Height];

        bitmapInfo = GDIHelper.CreateBitmapinfo(width, height);
    }

    public void SetPixel(int x, int y, Color color)
    {
        int index = (x + y * width);

        if (index >= 0 && index < data.Length)
        {
            data[index] = color.rgb;
        }
    }

    internal int GetBufferSize()
    {
        return data.Length;
    }

    public int GetPixelUnsafe(int index)
    {
        return data[index];
    }

    public void SetPixelUnsafe(int index, Color color)
    {
        data[index] = color.rgb;
    }

    public void SetPixelUnsafe(int index, int color)
    {
        data[index] = color;
    }

    public void PaintBitmap(HandleRef hRef)
    {
        if (width == 0 || height == 0)
        {
            return;
        }

        CIntegrations.SetDIBitsToDevice(hRef, 0, 0, width, height, 0, 0, 0, height, ref data[0], ref bitmapInfo, 0);
    }

    public void Blend(Color[] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            int color = data[i];
            //Color light = map[i];

            data[i] = map[i].Multiply(
                color >> 16 & 255,
                color >> 8 & 255,
                color & 255);
        }
    }

    //public Bitmap Convert()
    //{
    //    BitmapData b_data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, format);

    //    Marshal.Copy(data, 0, b_data.Scan0, data.Length);

    //    image.UnlockBits(b_data);

    //    return image;
    //}
}

