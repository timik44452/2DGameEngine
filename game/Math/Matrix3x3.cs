public struct Matrix3x3
{
    public float
        m00, m10, m20,
        m01, m11, m21,
        m02, m12, m22;

    public Matrix3x3(
        float m00, float m10, float m20,
        float m01, float m11, float m21,
        float m02, float m12, float m22)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;

        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;

        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
    }

    public Matrix3x3(
        double m00, double m10, double m20,
        double m01, double m11, double m21,
        double m02, double m12, double m22)
    {
        this.m00 = (float)m00;
        this.m01 = (float)m01;
        this.m02 = (float)m02;
                   
        this.m10 = (float)m10;
        this.m11 = (float)m11;
        this.m12 = (float)m12;
                  
        this.m20 = (float)m20;
        this.m21 = (float)m21;
        this.m22 = (float)m22;
    }

    public static Vector Multiply(Matrix3x3 a, Vector b)
    {
        b.X = a.m00 * b.X + a.m10 * b.Y + a.m20;
        b.Y = a.m01 * b.X + a.m11 * b.Y + a.m21;

        return b;
    }
}