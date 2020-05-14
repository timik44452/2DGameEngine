using Graphics.Gizmos;

public class Line : Primitive
{
    public Vector from;
    public Vector to;

    public Line(Vector from, Vector to)
    {
        this.from = from;
        this.to = to;
    }

    public override void Draw(Matrix3x3 viewMatrix, int width, int height, ref int[] frame)
    {
        int color = this.color.ToInt();

        Vector viewFrom = Matrix3x3.Multiply(viewMatrix, from);
        Vector viewTo = Matrix3x3.Multiply(viewMatrix, to);

        float step = 1.0F / Vector.Distance(viewFrom, viewTo);
      
        for (float alpha = 0; alpha < 1.0F; alpha += step)
        {
            Vector point = Vector.Lerp(viewFrom, viewTo, alpha);

            int x = (int)point.X;
            int y = (int)point.Y;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                frame[x + y * width] = color;
            }
        }
    }
}