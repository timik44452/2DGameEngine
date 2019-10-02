public class Camera
{
    public Camera(Rect rect)
    {
        viewport = rect;
    }

    public Vector position { get; set; }
    public Rect viewport { get; private set; }

}