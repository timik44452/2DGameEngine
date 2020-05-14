public class Camera : Component
{
    public Rect viewport { get; set; }

    public void Renderer()
    {
        GraphicCore.currentGraphic.DrawGameObjects(this, gameObject.world.GetViewedObjects());
    }
}