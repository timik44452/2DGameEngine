// GraphicCore.cpp : Определяет функции для статической библиотеки.
//
#pragma once
#include "pch.h"
#include "DirectXCore.h"

static DirectXCore* core;

extern "C" __declspec (dllexport) void _stdcall Init()
{
	core = new DirectXCore();
}

extern "C" __declspec (dllexport) int _stdcall InitDevice(int Width, int Height, HWND context)
{
	core = new DirectXCore();
	return core->DXInitDevice(Width, Height, context);
}

extern "C" _declspec (dllexport) int _stdcall SetViewport(int Width, int Height)
{
	return core->DXSetViewport(Width, Height);
}

extern "C" _declspec (dllexport) int _stdcall LoadShaders(
	const char* pixelShaderPath,
	const char* vertexShaderPath)
{
	return core->DXLoadShaders(pixelShaderPath, vertexShaderPath);
}

extern "C" __declspec (dllexport) void _stdcall Draw(int verticesCount, Vertex* vertices)
{
	core->DXDraw(verticesCount, vertices);
}

extern "C" __declspec (dllexport) void _stdcall Release()
{
	core->DXRelease();
}