using System;


public struct Vector
{
    private float x;
    private float y;
    private float length;

    public static Vector right { get => new Vector(1, 0); }
    public static Vector up { get => new Vector(0, 1); }
    public static Vector one { get => new Vector(1, 1); }
    public static Vector zero { get => new Vector(0, 0); }

    public float X
    {
        get => x;
        set
        {
            x = value;
            Recalculate();
        }
    }
    public float Y
    {
        get => y;
        set
        {
            y = value;
            Recalculate();
        }
    }

    public float Length
    {
        get => length;
    }

    public Vector Normalized
    {
        get => this / length;
    }

    public Vector(double X, double Y)
    {
        x = (float)X;
        y = (float)Y;

        length = 0;

        Recalculate();
    }

    public Vector(Point point)
    {
        x = point.x;
        y = point.y;

        length = 0;

        Recalculate();
    }

    public Vector(float X, float Y)
    {
        x = X;
        y = Y;

        length = 0;

        Recalculate();
    }

    private void Recalculate()
    {
        length = (float)Math.Sqrt(x * x + y * y);
    }

    public static Vector Lerp(Vector from, Vector to, float t)
    {
        return from + (to - from) * t;
    }

    public static float Distance(Vector a, Vector b)
    {
        return (a - b).Length;
    }

    public static Vector operator -(Vector a)
    {
        return a *= -1;
    }
    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y);
    }
    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }
    public static Vector operator *(float b, Vector a)
    {
        return new Vector(a.X * b, a.Y * b);
    }
    public static Vector operator *(Vector a, float b)
    {
        return new Vector(a.X * b, a.Y * b);
    }
    public static Vector operator /(Vector a, float b)
    {
        return new Vector(a.X / b, a.Y / b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector)
        {
            Vector v = (Vector)obj;

            return
                 v.X == X &&
                 v.Y == Y;
        }

        return false;
    }
}