public interface IGraphic
{
    void Draw();
    void DrawGameObjects(Camera camera, GameObject[] gameObjects);
    void Release();
}