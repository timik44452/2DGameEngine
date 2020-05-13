public struct Matrix2x2
{
    public float 
        m00, m10,
        m01, m11;

    public float 
        row0, 
        row1;

    public float column0, column1;

    public Matrix2x2(
        float m00, float m10,
        float m01, float m11)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m10 = m10;
        this.m11 = m11;

        row0 = m00 + m10;
        row1 = m01 + m11;

        column0 = m00 + m01;
        column1 = m10 + m11;
    }

    public static Vector Multiply(Matrix2x2 a, Vector b)
    {
        b.X = a.row0 * b.X;
        b.Y = a.row1 * b.Y;

        return b;
    }
}