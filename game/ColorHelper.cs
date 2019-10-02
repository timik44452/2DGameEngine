public static class ColorHelper
{
    public static Color GetColor(byte R, byte G, byte B, byte A)
    {
        return new Color(R / 255F, G / 255F, B / 255F, A / 255F);
    }

    public static int GetColor(int r, int g, int b)
    {
        return System.Drawing.Color.FromArgb(r, g, b).ToArgb();
    }

    public static byte GetR(byte color)
    {
        if ((color & 1 << 7) > 0 && (color & 1 << 6) > 0) return 1 << 7 | 1 << 6;
        else
        if ((color & (1 << 7)) > 0) return 1 << 7;
        else
        if ((color & (1 << 6)) > 0) return 1 << 6;
        else
            return 0;
    }

    public static Color GetColor(int color)
    {
        var c = System.Drawing.Color.FromArgb(color);
        return new Color(c.R / 255F, c.G / 255F, c.B / 255F, c.A / 255F);
    }

    public static byte GetG(byte color)
    {
        if ((color & 1 << 5) > 0 && (color & 1 << 4) > 0) return 1 << 7 | 1 << 6;
        else
        if ((color & (1 << 5)) > 0) return 1 << 7;
        else
        if ((color & (1 << 4)) > 0) return 1 << 6;
        else
            return 0;
    }

    public static int GetColor(float R, float G, float B)
    {
        return System.Drawing.Color.FromArgb((int)(R * 255), (int)(G * 255), (int)(B * 255)).ToArgb();
    }

    public static byte GetB(byte color)
    {
        if ((color & 1 << 3) > 0 && (color & 1 << 2) > 0) return 1 << 7 | 1 << 6;
        else
        if ((color & (1 << 3)) > 0) return 1 << 7;
        else
        if ((color & (1 << 2)) > 0) return 1 << 6;
        else
            return 0;
    }

    public static int Multiply(float value, Color color)
    {
        //int r = (color >> 16) & 255;
        //int g = (color >> 8) & 255;
        //int b = color & 255;

        //r = (int)(r * light);
        //g = (int)(g * light);
        //b = (int)(b * light);

        //int c = r << 16 | g << 8 | b;
        return color.rgb;
        //
    }
}

