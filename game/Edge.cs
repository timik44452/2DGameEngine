
public class Edge
{
    public Vector Start;
    public Vector End;
    public Vector Normal;

    public Edge(Vector start, Vector end)
    {
        Start = start;
        End = end;

        Normal = new Vector(-(Start.Y - End.Y), -(Start.X - End.X));
    }
}
