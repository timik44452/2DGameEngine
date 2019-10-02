
public static class Compressor
{
    public static short Compress(byte R, byte G, byte B, byte A)
    {
        int r = R / 25;
        int g = G / 25;
        int b = B / 25;
        int a = A / 25;

        return (short)(a * 1000 + r * 100 + g * 10 + b);
    }

    public static int Decompress(short value)
    {
        int r = value / 100;
        int g = value / 10 % 10;
        int b = value % 10;

        return ColorHelper.GetColor(r * 25, g * 25, b * 25);
    }
    public static Color DecompressColor(short value)
    {
        if (value == 0)
            return ColorAtlas.Transparent;

        int r = value / 100;
        int g = value / 10 % 10;
        int b = value % 10;

        return new Color(r * 0.1F, g * 0.1F, b * 0.1F, 1F);
    }

    public static Texture Decompress(int width, int height, short[] value)
    {
        Texture result = new Texture(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                result.SetPixel(x, y, DecompressColor(value[x + y * width]));
            }

        return result;
    }
}

