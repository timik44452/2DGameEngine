
using System.Runtime.InteropServices;

public static class CIntegrations
{
    public struct Color
    {
        private float r;
        private float g;
        private float b;
        private float a;

        public int rgb;

        public Color(float r, float g, float b, float a, int rgb)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.rgb = rgb;
        }
    }

    [DllImport("gdi32")]
    public extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, ref int lpvBits, ref GDIHelper.BITMAPINFO lpbmi, uint fuColorUse);

    [DllImport("AMPCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void draw_sprite(ref int frame, ref Color sprite, ref int zBuffer, int stride, int layer, int w, int h, int sw, int sh, int bs);

    [DllImport("AMPCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void clear(ref int buffer, int value, int count);

    [DllImport("AMPCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int input_handler(int code);

    //[DllImport("AMPCore", CallingConvention = CallingConvention.StdCall)]
    //public extern static Point get_mouse_position();
}