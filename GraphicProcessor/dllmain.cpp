// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <math.h>

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

struct Color
{
public:
    float r, g, b, a;
    int RInt32, GInt32, BInt32;
};

int* frame;
int* depth;

int width;
int height;

extern "C" __declspec (dllexport) void _stdcall init(int* _frame, int* _depth, int _width, int _height)
{
    frame = _frame;
    depth = _depth;

    width = _width;
    height = _height;
}

extern "C" __declspec (dllexport) void _stdcall draw(float x, float y, float w, float h, Color* sprite, int sw, int sh)
{
    int start_x = (int)(x - w * 0.5F);
    int start_y = (int)(y - h * 0.5F);

    int end_x = (int)(x + w * 0.5F);
    int end_y = (int)(y + h * 0.5F);

    int sl = sw * sh;

    float uv_to_width = sw / (float)abs(start_x - end_x);
    float uv_to_height = sh / (float)abs(start_y - end_y);

    for (int y = start_y; y < end_y; y++)
    {
        for (int x = start_x; x < end_x; x++)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                int screen_idx = (x + y * width);
                int uv_idx =
                    (int)((x - start_x) * uv_to_width) +
                    (int)((y - start_y) * uv_to_height) * sh;


                if (uv_idx < sl)
                {
                    //float alpha0 = frame[screen_idx]
                    Color color = sprite[uv_idx];

                    frame[screen_idx] = color.RInt32;
                }
            }
        }
    }
}

extern "C" __declspec (dllexport) void _stdcall clear()
{
    memset(frame, 0, width * height * 4);
}