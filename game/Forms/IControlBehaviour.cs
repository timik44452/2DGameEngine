public interface IControlBehaviour
{
    float deltaTime { get; }

    void OnLoad(object sender);
    void OnClosed(object sender);
    void OnPaint(object sender);
}
