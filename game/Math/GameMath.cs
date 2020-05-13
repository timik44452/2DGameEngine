using System;

public static class GameMath
{
    public static float Max(float value, float max)
    {
        return (value > max) ? value : max;
    }

    public static float Min(float value, float min)
    {
        return (value < min) ? value : min;
    }

    public static float Clamp(float value, float min, float max)
    {
        if (value > max)
        {
            return max;
        }
        else if (value < min)
        {
            return min;
        }
        else
        {
            return value;
        }
    }

    public static float Abs(float value)
    {
        if (value < 0)
            return -value;
        else
            return value;
    }

}
