using System;
using System.Runtime.InteropServices;

public static class GraphicCore
{
    public static IGraphic currentGraphic
    {
        get
        {
            if(s_currentGraphic == null)
            {
                throw new Exception("Graphic context hasn't initialized");
            }

            return s_currentGraphic;
        }
    }

    private static IGraphic s_currentGraphic;

    public static void Initialize(int width ,int height, HandleRef handle)
    {
        s_currentGraphic = GDIGraphic.Create(width, height, handle);
    }
}