public struct Color
{
    public float r;
    public float g;
    public float b;
    public float a;

    public int RInt32;
    public int GInt32;
    public int BInt32;
    public int RGBInt32;

    public const int sizeofColor = sizeof(float) * 4 + sizeof(int) * 5;

    public Color(byte R, byte G, byte B)
    {
        r = R / 255F;
        g = G / 255F;
        b = B / 255F;
        a = 1;

        RInt32 = (int)(r * 255);
        GInt32 = (int)(g * 255);
        BInt32 = (int)(b * 255);
        RGBInt32 = 0;
        RGBInt32 = ToInt();
    }

    public Color(float R, float G, float B)
    {
        r = R;
        g = G;
        b = B;
        a = 1;

        RInt32 = (int)(r * 255);
        GInt32 = (int)(g * 255);
        BInt32 = (int)(b * 255);
        RGBInt32 = 0;
        RGBInt32 = ToInt();
    }

    public Color(float R, float G, float B, float A)
    {
        r = R;
        g = G;
        b = B;
        a = A;

        RInt32 = (int)(r * 255);
        GInt32 = (int)(g * 255);
        BInt32 = (int)(b * 255);
        RGBInt32 = 0;
        RGBInt32 = ToInt();
    }

    public int ToInt()
    {
        return
            255 << 24 |
            RInt32 << 16 |
            GInt32 << 8 |
            BInt32;
    }
}