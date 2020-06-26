using Graphics.Gizmos;

public interface IGraphic
{
    void BeginRenderer(Camera camera);
    void EndRenderer();
    void Draw(Vector position, Vector scale, Sprite sprite, bool useWorldSpace);
    void Release();
}