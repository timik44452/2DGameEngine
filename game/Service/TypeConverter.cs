
public static class TypeConverter
{
    public static Vertex VectorToVertex(Vector value)
    {
        return new Vertex()
        {
            x = value.X,
            y = value.Y,
            z = 0
        };
    }
    public static void VectorToVertex(Vector value, ref Vector source)
    {
        source.X = value.X;
        source.Y = value.Y;
    }
}