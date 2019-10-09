public class Camera : Component
{
    public Camera()
    {
        //viewport = rect;
    }

    public Rect viewport { get; set; }


    public void Renderer()
    {
        Graphic.graphics.DrawGameObjects(this, gameObject.world.GetViewedObjects());
    }
}