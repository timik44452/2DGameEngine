using System;

//TODO: Optimize
public struct Vector
{
    private float x;
    private float y;
    private float length;

    public static Vector right 
    {
        get => new Vector(1, 0); 
    }
    public static Vector up 
    {
        get => new Vector(0, 1); 
    }
    public static Vector one 
    { 
        get => new Vector(1, 1); 
    }
    public static Vector zero 
    { 
        get => new Vector(0, 0); 
    }

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

    public static float Dot(Vector a, Vector b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float Angle(Vector a, Vector b)
    {
        if(a.Length == 0 || b.Length == 0)
        {
            return 0;
        }

        return (float)Math.Acos(Dot(a, b) / a.Length * b.Length);
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
        a.x -= b.x;
        a.y -= b.y;

        a.Recalculate();

        return a;
    }
    public static Vector operator +(Vector a, Vector b)
    {
        a.x += b.X;
        a.y += b.y;

        a.Recalculate();

        return a;
    }
    public static Vector operator *(float b, Vector a)
    {
        a.x *= b;
        a.y *= b;

        a.Recalculate();

        return a;
    }
    public static Vector operator *(Vector a, float b)
    {
        a.x *= b;
        a.y *= b;

        a.Recalculate();

        return a;
    }
    public static Vector operator /(Vector a, float b)
    {
        a.x /= b;
        a.y /= b;

        a.Recalculate();

        return a;
    }
    public override string ToString()
    {
        return $"{X};{Y}";
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

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }
}