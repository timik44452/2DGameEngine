
public class Transform : Component
{
    public Vector position;
    public Vector scale = Vector.one;

    public int VerticalOrientation
    {
        get => (int)(scale.Y / System.Math.Abs(scale.Y));
    }
    public int HorizontalOrientation
    {
        get => (int)(scale.X / System.Math.Abs(scale.X));
    }
}