using System;

public static class GameDebug
{
    private enum ERROR_CODE
    {
        CREATE_TEXTURE_ERROR = 0x010101,
        CREATE_SHADER_RESOURCE_ERROR = 0x010102,
        LOAD_TEXTURE_ERROR = 0x010100,
        PIXEL_SHADER_COMPILE_ERROR = 0x00101,
        VERTEX_SHADER_COMPILE_ERROR = 0x00102,
        PIXEL_SHADER_LOADING_ERROR = 0x00111,
        VERTEX_SHADER_LOADING_ERROR = 0x00112,
        CREATE_VERTEX_BUFFER_ERROR = 0x00113,
        CREATE_INPUT_LAYOUT_ERROR = 0x00114,
        CREATE_DEVICE_AND_SWAP_CHAIN_ERROR = 0x00115,
        CREATE_BACK_BUFFER_ERROR = 0x00116,
        CREATE_RENDERER_TARGET_ERROR = 0x00117,

        NO_ERROR_CODE = 0
    };

    public static void Log(object message)
    {
        Console.WriteLine(message.ToString());
    }
    public static void DXLog(int code)
    {
       Log($"DirectX error:{(ERROR_CODE)code}");
    }
}