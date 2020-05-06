using System;

public class Shape
{
    public Vector center { get; private set; }
    public Vector[] vertexes { get; private set; }
    public int[] links { get; private set; }

    public Shape(int[] links, Vector[] vertexes)
    {
        this.links = links;
        this.vertexes = vertexes;

        Recalculate();
    }

    public void Recalculate()
    {
        center = Vector.zero;

        for (int i = 0; i < vertexes.Length; i++)
        {
            center += vertexes[i];
        }

        if (vertexes.Length > 0)
            center /= vertexes.Length;
    }

    public void CopyTo(int width, int height, bool[] collision_buffer)
    {
        if (width * height > collision_buffer.Length)
        {
            throw new Exception("Ты шо делаешь дебил ?");
        }

        for (int i = 1; i < links.Length; i++)
        {
            Vector v0 = vertexes[links[i - 1]];
            Vector v1 = vertexes[links[i]];

            for (float alpha = 0; alpha < 1F; alpha += 0.01F)
            {
                Vector point = Vector.Lerp(v0, v1, alpha);

                int x = (int)((width - 1) * (point.X + 1) / 2F);
                int y = (int)((height - 1) * (point.Y + 1) / 2F);

                //if (x >= 0 && x < width && y >= 0 && y < height)
                if (x + y * width < collision_buffer.Length)
                    collision_buffer[x + y * width] = true;
            }
        }

        #region Trace
        if (vertexes.Length > 2)
        {
            for (int y = 0; y < height; y++)
            {
                bool value = false;

                for (int x = 0; x < width; x++)
                {
                    if (collision_buffer[x + y * width])
                        value = !value;

                    collision_buffer[x + y * width] = value;
                }
            }
        }
        #endregion
    }
}