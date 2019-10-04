public enum MouseButton
{
    LeftButton = 0x01,
    RightButton = 0x02,
    MiddleButton = 0x04
}

public class Input
{
    public static Vector MousePosition;
    public static Vector WorldMousePosition;

    public static bool IsKeyPress(string Name)
    {
        return false;// (CIntegrations.input_handler(Name[0]) & 0x8000) == 0x8000;
    }

    public static bool IsMousePress(MouseButton mouseButton)
    {
        return false;//(CIntegrations.input_handler((int)mouseButton) & 0x8000) == 0x8000;
    }
}