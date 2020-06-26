using System;
using System.Runtime.InteropServices;

public static class GraphicsAPI
{
    public static IGraphic currentGraphic
    {
        get => s_currentGraphic ?? throw new Exception("Graphic context hasn't initialized");
    }

    private static IGraphic s_currentGraphic;

    public static void Initialize(int width, int height, HandleRef handle)
    {
        s_currentGraphic = GDIGraphic.Create(width, height, handle);
    }
}