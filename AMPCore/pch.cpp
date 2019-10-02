// pch.cpp: файл исходного кода, соответствующий предварительно скомпилированному заголовочному файлу

#include "pch.h"
#include <amp.h>
#include "Color.h"
#include <Windows.h>

using namespace concurrency;

extern "C" __declspec (dllexport) void _stdcall multiply_rgb(int count, int* data, float* r, float* g, float* b)
{
	array_view<float, 1> r_array(count, r);
	array_view<float, 1> g_array(count, g);
	array_view<float, 1> b_array(count, b);

	array_view<int, 1> data_array(count, data);

	parallel_for_each(data_array.extent, [=](index<1> idx) restrict(amp) 
	{
		int color = data_array[idx];

		data_array[idx] = 
			(int)((color >> 16 & 255) * r_array[idx]) |
			(int)((color >> 8 & 255) * g_array[idx]) |
			(int)((color & 255) * b_array[idx]);
	});

	data_array.synchronize();
}

extern "C" __declspec (dllexport) void _stdcall calc_lightmap(int count, int* data)
{
	array_view<int, 1> data_array(count, data);

	/*parallel_for_each(data_array.extent, [=](index<1> idx) restrict(amp)
	{

	});
*/
	data_array.synchronize();
}
extern "C" __declspec (dllexport) void _stdcall draw_sprite(int* frame, Color* sprite, int* zBuffer, int stride, int layer, int w, int h, int sw, int sh, int bs)
{
	int i = 0;

	int to_y = sh / 2;
	int to_x = sw / 2;

	int from_y = -to_y;
	int from_x = -to_x;

	if (from_x > to_x)std::swap(from_x, to_x);
	if (from_y > to_y)std::swap(from_y, to_y);


	if (sw > 0)
	{
		for (int y = from_y; y < to_y; y++)
		{
			int _y = stride + y * w;

			for (int x = from_x; x < to_x; x++)
			{
				int _f_i = _y + x;

				if (_f_i > 0 && _f_i < bs && sprite[i].a > 0 && zBuffer[_f_i] < layer)
				{
					frame[_f_i] = sprite[i].rgb;
					//zBuffer[_f_i] = layer;
				}

				i++;
			}
		}

		memset(zBuffer, layer, sw * sh);
	}
	else
	{
		for (int y = from_y; y < to_y; y++)
		{
			int _y = stride + y * w;

			for (int x = from_x; x < to_x; x++)
			{
				int _f_i =  _y - x;

				if (_f_i > 0 && _f_i < bs && sprite[i].a > 0 && zBuffer[_f_i] < layer)
				{
					frame[_f_i] = sprite[i].rgb;
					//zBuffer[_f_i] = layer;
				}

				i++;
			}

		}
	}
}
extern "C" __declspec (dllexport) void _stdcall clear(int* buffer, int value, int size)
{
	/*for (int i = 0; i < size; i++)
		buffer[i] = value;*/
	memset(buffer, value, size * 4);
}
extern "C" __declspec (dllexport) int _stdcall input_handler(int code)
{
	return GetAsyncKeyState(code);
}
extern "C" __declspec (dllexport) void _stdcall get_mouse_position()
{

}