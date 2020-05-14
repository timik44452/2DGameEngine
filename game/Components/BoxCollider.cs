
using System;

public class BoxCollider : Collider
{
    public Vector[] Points;
    public Edge[] Edges;

    public BoxCollider(Vector first, Vector second, Vector third, Vector fourth)
    {
        Points = new Vector[4];
        Edges = new Edge[4];

        Points[0] = first;
        Points[1] = second;
        Points[2] = third;
        Points[3] = fourth;

        for (int i = 0; i < Edges.Length - 1; i++)
            Edges[i] = new Edge(Points[i], Points[i + 1]);

        Edges[Edges.Length - 1] = new Edge(Points[Points.Length - 1], Points[0]);

        float maxRadius;
        Radius = (float) Math.Sqrt(Math.Pow(Points[0].X, 2) + Math.Pow(Points[0].Y, 2));
        foreach(Vector point in Points)
        {
            maxRadius = (float)Math.Sqrt(Math.Pow(Points[0].X, 2) + Math.Pow(Points[0].Y, 2));
            if (maxRadius > Radius)
                Radius = maxRadius;
        }
    }
}
