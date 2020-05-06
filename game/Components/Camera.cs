public class Camera : Component
{
    public Camera()
    {
        //viewport = rect;
    }

    public Rect viewport { get; set; }


    public void Renderer()
    {
        GraphicCore.currentGraphic.DrawGameObjects(this, gameObject.world.GetViewedObjects());
    }
}