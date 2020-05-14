namespace Graphics.Gizmos
{
    public abstract class Primitive
    {
        public Color color { get; set; }

        public abstract void Draw(Matrix3x3 viewMatrix, int width, int height, ref int[] frame);
    }
}
