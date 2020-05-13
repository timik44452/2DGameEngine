// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <amp.h>

using namespace Concurrency;

BOOL APIENTRY DllMain( HMODULE hModule,
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
struct Vector
{
public:
    float x;
    float y;
    float l;
};

extern "C" __declspec (dllexport) void _stdcall draw(int* arr, int size, int* sprite, int spsize, Vector* vectors, int count)
{
    array_view<Vector, 1> _vectors(count, &vectors[0]);
    array_view<int, 1> array(size, &arr[0]);

    /*parallel_for_each(array.extent, [=](index<1> idx) restrict(amp)
    {
        array[idx] = value;
    });*/

    parallel_for_each(_vectors.extent, [=](index<1> idx) restrict(amp)
    {
        Vector v = _vectors[idx];

        for (int x = 0; x < 512; x++)
        {
            array[x] = 255;
        }
    });

    //for (int y = start_y; y < end_y; y++)
    //{
    //    for (int x = start_x; x < end_x; x++)
    //    {
    //        if (x >= 0 && x < width && y >= 0 && y < height)
    //        {
    //            int screen_idx = (x + y * width);
    //            int uv_idx =
    //                (int)((x - start_x) * uv_to_width) +
    //                (int)((y - start_y) * uv_to_height) * sprite.Height;
    //        }
    //    }
    //}


    array.synchronize();
}