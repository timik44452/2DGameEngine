public enum Space
{
    World,
    Screen
}

public class Transform : Component
{
    public Space space = Space.World;
    public Vector position
    {
        get => GetPropertyValue<Vector>(nameof(position));
        set => SetPropertyValue(nameof(position), value);
    }
    public Vector scale
    {
        get => GetPropertyValue<Vector>(nameof(scale));
        set => SetPropertyValue(nameof(scale), value);
    }

    public override void OnCreated()
    {
        CreateProperty(nameof(position), Vector.zero);
        CreateProperty(nameof(scale), Vector.one);
    }
}