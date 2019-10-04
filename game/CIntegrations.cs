
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
    public struct Vertex
    {
        public float x, y, z;
    }

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int InitDevice(int Width, int Height, System.IntPtr context);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int SetViewport(int Width, int Height);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int LoadShaders(ref char pixelShaderPath, ref char vertexShaderPath);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void Draw(int verticesCount, ref Vertex vertices);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void Release();

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void Init();
}