using System;

public class LightMap
{
    public float[] RlightMap;
    public float[] GlightMap;
    public float[] BlightMap;

    private int size;
    private int width;
    private int height;


    public LightMap(int width, int height)
    {
        size = width * height;

        this.width = width;
        this.height = height;

        RlightMap = new float[size];
        GlightMap = new float[size];
        BlightMap = new float[size];
    }

    public void AddLight(int x, int y, float RValue, float GValue, float BValue)
    {
        if (x < width && x >= 0 && y < height && y > 0)
        {
            int index = x + y * width;

            RlightMap[index] += RValue;
            GlightMap[index] += GValue;
            BlightMap[index] += BValue;
        }
    }

    public void Clear(Color value)
    {
        for (int i = 0; i < size; i++)
        {
            RlightMap[i] = value.R;
            GlightMap[i] = value.G;
            BlightMap[i] = value.B;
        }
    }

    public void AddLight(float R, float G, float B)
    {
        for (int i = 0; i < size; i++)
        {
            RlightMap[i] += R;
            GlightMap[i] += G;
            BlightMap[i] += B;
        }
    }

    public int GetMapSize()
    {
        return size;
    }

    public float GetR(int i)
    {
        return RlightMap[i];
    }

    public float GetG(int i)
    {
        return GlightMap[i];
    }

    public float GetB(int i)
    {
        return BlightMap[i];
    }
}