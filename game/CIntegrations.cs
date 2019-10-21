using System.Runtime.InteropServices;

public static class CIntegrations
{
    public static int CreateDXResource(Sprite sprite)
    {
        return LoadTextureFromInt(ref sprite.Buffer[0], sprite.UID, sprite.Width, sprite.Height);
    }

    public static void Draw(int vertexIndex, int vertexCount, Sprite sprite)
    {
        Draw(vertexIndex, vertexCount, sprite.UID);
    }

    public static void ClearBackground(Color color)
    {
        ClearBackground(color.R, color.G, color.B);
    }

    public static int UpdateVertexBuffer(Vertex[] vertices)
    {
        return UpdateBuffer(vertices.Length, ref vertices[0]);
    }


    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int InitDevice(int Width, int Height, System.IntPtr context);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int SetViewport(int Width, int Height);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static int LoadShaders(
        [MarshalAs(UnmanagedType.LPWStr)]
        string pixelShaderPath,
        [MarshalAs(UnmanagedType.LPWStr)]
        string vertexShaderPath);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    public extern static void Release();


    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    private extern static int UpdateBuffer(int verticesCount, ref Vertex vertices);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    private extern static int LoadTextureFromInt(ref int bytes, uint textureSlot, int width, int height);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    private extern static void Draw(int startIndex, int verticesCount, uint textureSlot);

    [DllImport("GraphicCore", CallingConvention = CallingConvention.StdCall)]
    private extern static void ClearBackground(float r, float g, float b);
}