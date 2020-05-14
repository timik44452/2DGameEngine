using Graphics.Gizmos;

public static class Gizmos
{
    public static Primitive[] primitives { get; private set; } = new Primitive[0];

    public static int currentPrimitive = 0;

    public static void DrawLine(Vector from, Vector to, Color color)
    {
        DrawPrimitive(new Line(from, to) { color = color });
    }

    public static void DrawPrimitive(Primitive primitive)
    {
        if (primitives.Length <= currentPrimitive)
        {
            var tempBuffer = primitives;
            primitives = new Primitive[currentPrimitive + 1];
            tempBuffer.CopyTo(primitives, 0);
        }

        primitives[currentPrimitive] = primitive;

        currentPrimitive++;
    }
}