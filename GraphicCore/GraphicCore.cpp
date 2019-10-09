// GraphicCore.cpp : Определяет функции для статической библиотеки.
//
#pragma once
#include "pch.h"
#include "DirectXCore.h"

static DirectXCore* core;


extern "C" __declspec (dllexport) int _stdcall InitDevice(int Width, int Height, HWND context)
{
	core = new DirectXCore();
	return core->DXInitDevice(Width, Height, context);
}

extern "C" _declspec (dllexport) int _stdcall LoadShaders(
	LPCWSTR pixelShaderPath,
	LPCWSTR vertexShaderPath)
{
	return  core->DXLoadShaders(pixelShaderPath, vertexShaderPath);
}

extern "C" _declspec (dllexport) int _stdcall LoadTextureFromInt(int* src, int width, int height)
{
	return core->LoadTexture(src, width, height);
}

extern "C" __declspec (dllexport) void _stdcall Draw(int verticesCount)
{
	core->DXDraw(verticesCount);
}
extern "C" __declspec (dllexport) int _stdcall UpdateBuffer(int verticesCount, Vertex * vertices)
{
	return core->DXUpdateVertexBuffer(verticesCount, vertices);
}
extern "C" __declspec (dllexport) void _stdcall Release()
{
	core->DXRelease();
}