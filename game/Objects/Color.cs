﻿public struct Color
{
    public float R
    {
        get => r;
        set
        {
            r = GameMath.Clamp(value, 0, 1);
            RInt32 = (int)(r * 255);
        }
    }
    public float G
    {
        get => g;
        set
        {
            g = GameMath.Clamp(value, 0, 1);
            GInt32 = (int)(g * 255);
        }
    }
    public float B
    {
        get => b;
        set
        {
            b = GameMath.Clamp(value, 0, 1);
            BInt32 = (int)(b * 255);
        }
    }
    public float A
    {
        get => a;
        set => a = GameMath.Clamp(value, 0, 1);
    }

    private float r;
    private float g;
    private float b;
    private float a;

    private int RInt32;
    private int GInt32;
    private int BInt32;

    public Color(byte R, byte G, byte B)
    {
        r = R / 255F;
        g = G / 255F;
        b = B / 255F;
        a = 1;

        RInt32 = (int)(r * 255);
        GInt32 = (int)(g * 255);
        BInt32 = (int)(b * 255);
    }

    public Color(byte R, byte G, byte B, byte A)
    {
        r = R / 255F;
        g = G / 255F;
        b = B / 255F;
        a = A / 255F;

        RInt32 = (int)(r * 255);
        GInt32 = (int)(g * 255);
        BInt32 = (int)(b * 255);
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
    }

    public int GetRGB(int R, int G, int B)
    {
        int r = R << 16;
        int g = G << 8;
        int b = B;

        int result = 0;

        result |= b;
        result |= g;
        result |= r;

        return result;
    }

    public int GetRInt32()
    {
        return RInt32;
    }

    public int GetGInt32()
    {
        return GInt32;
    }

    public int GetBInt32()
    {
        return BInt32;
    }
}