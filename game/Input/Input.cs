public enum MouseButton
{
    LeftButton = 0x01,
    RightButton = 0x02,
    MiddleButton = 0x04
}

public static class Input
{
    public static Vector MousePosition
    {
        get
        {
            if (inputContext == null)
                return Vector.zero;

            return inputContext.mousePosition;
        }
    }

    private static InputContext inputContext;

    public static bool IsKeyPress(string Name)
    {
        var state = inputContext.GetKeyState(Name);

        return
            state == KeyState.KeyDown ||
            state == KeyState.Pressed;
    }

    public static bool IsMousePress(MouseButton mouseButton)
    {
        return false;//(CIntegrations.input_handler((int)mouseButton) & 0x8000) == 0x8000;
    }

    public static void Initialize(InputContext context)
    {
        inputContext = context;
    }
}