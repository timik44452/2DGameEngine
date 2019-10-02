public class Light : Component
{
    public enum LightType
    {
        Directional,
        Form
    }

    public float radius = 1;
    public float intensity = 1F;
    public Color color = ColorAtlas.White;


    public LightType lightType = LightType.Directional;
}

