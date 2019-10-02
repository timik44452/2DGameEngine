using System;
using System.Runtime.InteropServices;


public static class GDIHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public int bihSize;
        public int bihWidth;
        public int bihHeight;
        public short bihPlanes;
        public short bihBitCount;
        public int bihCompression;
        public int bihSizeImage;
        public double bihXPelsPerMeter;
        public double bihClrUsed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER biHeader;
        public int biColors;
    }

    public unsafe static BITMAPINFO CreateBitmapinfo(int width, int height)
    {
        int bytesPerPixel = 4;
        int stride = width * bytesPerPixel + (width * bytesPerPixel) % 4;

        return new BITMAPINFO
        {
            biHeader =
                    {
                        bihBitCount = (short)(bytesPerPixel * 8),
                        bihPlanes = 1,
                        bihSize = sizeof(BITMAPINFO),
                        bihWidth = stride / bytesPerPixel,
                        bihHeight = -height,
                        bihSizeImage = stride * height
                    }
        };
    }
}

