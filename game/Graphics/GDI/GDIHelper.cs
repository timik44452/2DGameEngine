using System.Runtime.InteropServices;

namespace Graphics.GDI
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

    public static class GDIHelper
    {
        [DllImport("gdi32")]
        public extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, ref int lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

        public static BITMAPINFO CreateBitmapinfo(int width, int height)
        {
            int bytesPerPixel = 4;
            int stride = width * bytesPerPixel + (width * bytesPerPixel) % 4;

            return new BITMAPINFO
            {
                biHeader =
                    {
                        bihBitCount = (short)(bytesPerPixel * 8),
                        bihPlanes = 1,
                        bihSize = 40,
                        bihWidth = stride / bytesPerPixel,
                        bihHeight = -height,
                        bihSizeImage = stride * height
                    }
            };
        }
    }
}