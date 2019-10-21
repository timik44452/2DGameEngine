#pragma once
#include <windows.h>
#include <d3d11.h>
#include <D3DX11.h>
#include <map>
#include <algorithm>
#include <wrl/client.h>
#include <d3dcompiler.h>

using namespace Microsoft::WRL;

struct Vertex
{
	float x, y, z, u, v;
};

enum ERROR_CODE
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

	NO_ERROR_CODE = NOERROR
};

class DirectXCore
{
public:
	Vertex*							sys_vertices = nullptr;
	int								vertext_buffer_size = 0;


	D3D_FEATURE_LEVEL				g_featureLevel = D3D_FEATURE_LEVEL_11_0;

	std::map<int, ID3D11ShaderResourceView*>	textures;
	ID3D11Device*							device = nullptr;
	IDXGISwapChain*							swapChain = nullptr;
	ID3D11SamplerState*						samplerState = nullptr;
	ID3D11DeviceContext*					deviceContext = nullptr;
	ID3D11PixelShader*						g_pPixelShader = nullptr;
	ID3D11VertexShader*						g_pVertexShader = nullptr;
	ID3D11InputLayout*						g_pVertexLayout = nullptr;
	ID3D11Buffer*							g_pVertexBuffer = nullptr;
	ID3D11RenderTargetView*					g_pRenderTargetView = nullptr;


	int DXInitDevice(int Width, int Height, HWND context)
	{
		HRESULT hr = S_OK;

		UINT createDeviceFlags = 0;

		D3D_DRIVER_TYPE driverTypes[] =
		{
			D3D_DRIVER_TYPE_HARDWARE,
			D3D_DRIVER_TYPE_WARP,
			D3D_DRIVER_TYPE_REFERENCE,
		};

		D3D_FEATURE_LEVEL featureLevels[] =
		{
			D3D_FEATURE_LEVEL_11_0,
			D3D_FEATURE_LEVEL_10_1,
			D3D_FEATURE_LEVEL_10_0,
		};

		UINT numFeatureLevels = ARRAYSIZE(featureLevels);

		DXGI_SWAP_CHAIN_DESC sd = {};

		sd.BufferCount = 1;
		sd.BufferDesc.Width = Width;
		sd.BufferDesc.Height = Height;
		sd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		sd.BufferDesc.RefreshRate.Numerator = 60;
		sd.BufferDesc.RefreshRate.Denominator = 1;
		sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		sd.OutputWindow = context;
		sd.SampleDesc.Count = 1;
		sd.SampleDesc.Quality = 0;
		sd.Windowed = TRUE;

		for (UINT driverTypeIndex = 0; driverTypeIndex < ARRAYSIZE(driverTypes); driverTypeIndex++)
		{
			hr = D3D11CreateDeviceAndSwapChain(NULL, driverTypes[driverTypeIndex], NULL, createDeviceFlags, featureLevels, numFeatureLevels,
				D3D11_SDK_VERSION, &sd, &swapChain, &device, &g_featureLevel, &deviceContext);

			if (SUCCEEDED(hr))
				break;
		}

		if (FAILED(hr))
		{
			return CREATE_DEVICE_AND_SWAP_CHAIN_ERROR;
		}

		ID3D11Texture2D* pBackBuffer = NULL;

		hr = swapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&pBackBuffer);

		if (FAILED(hr))
		{
			return CREATE_BACK_BUFFER_ERROR;
		}

		hr = device->CreateRenderTargetView(pBackBuffer, NULL, &g_pRenderTargetView);
		pBackBuffer->Release();

		if (FAILED(hr))
		{
			return CREATE_RENDERER_TARGET_ERROR;
		}

		deviceContext->OMSetRenderTargets(1, &g_pRenderTargetView, NULL);

		// Setup the viewport
		D3D11_VIEWPORT vp;

		vp.Width = (FLOAT)Width;
		vp.Height = (FLOAT)Height;

		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;

		vp.TopLeftX = 0;
		vp.TopLeftY = 0;

		deviceContext->RSSetViewports(1, &vp);

		ChangeVertexBufferSize(0);

		deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		//textures = std::vector<ID3D11ShaderResourceView**>();

		return NO_ERROR_CODE;
	}

	int DXLoadShaders(
		LPCWSTR pixelShaderPath,
		LPCWSTR vertexShaderPath)
	{
		HRESULT hr = NULL;

		ID3DBlob* pVSBlob = NULL;
		ID3DBlob* pPSBlob = NULL;

		hr = D3DX11CompileFromFile(vertexShaderPath, NULL, NULL, "main", "vs_5_0", 0, 0, NULL, &pVSBlob, NULL, NULL);
		
		if (FAILED(hr))
		{
			return VERTEX_SHADER_COMPILE_ERROR;
		}

		hr = device->CreateVertexShader(pVSBlob->GetBufferPointer(), pVSBlob->GetBufferSize(), NULL, &g_pVertexShader);
		
		if (FAILED(hr))
		{
			return VERTEX_SHADER_LOADING_ERROR;
		}

		hr = D3DX11CompileFromFile(pixelShaderPath, NULL, NULL, "main", "ps_5_0", 0, 0, NULL, &pPSBlob, NULL, NULL);

		if (FAILED(hr))
		{
			return PIXEL_SHADER_COMPILE_ERROR;
		}

		hr = device->CreatePixelShader(pPSBlob->GetBufferPointer(), pPSBlob->GetBufferSize(), NULL, &g_pPixelShader);

		pPSBlob->Release();

		if (FAILED(hr))
		{
			return PIXEL_SHADER_LOADING_ERROR;
		}

		
		D3D11_INPUT_ELEMENT_DESC layout[] =
		{
			{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
			{ "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 }
		};

		UINT numElements = ARRAYSIZE(layout);

		// Create the input layout
		hr = device->CreateInputLayout(layout, numElements, pVSBlob->GetBufferPointer(),
			pVSBlob->GetBufferSize(), &g_pVertexLayout);

		D3D11_SAMPLER_DESC sampDesc = {};

		sampDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
		sampDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
		sampDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
		sampDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;
		sampDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
		sampDesc.MinLOD = 0;
		sampDesc.MaxLOD = D3D11_FLOAT32_MAX;

		hr = device->CreateSamplerState(&sampDesc, &samplerState);


		pVSBlob->Release();

		if (FAILED(hr))
		{
			return CREATE_INPUT_LAYOUT_ERROR;
		}

		deviceContext->IASetInputLayout(g_pVertexLayout);

		deviceContext->PSSetSamplers(0, 1, &samplerState);
		deviceContext->VSSetShader(g_pVertexShader, NULL, 0);
		deviceContext->PSSetShader(g_pPixelShader, NULL, 0);

		return NO_ERROR_CODE;
	}

	int LoadTexture(int* src, int textureSlot, int width, int height)
	{
		DXGI_FORMAT format = DXGI_FORMAT::DXGI_FORMAT_R8G8B8A8_UNORM;//::DXGI_FORMAT_R8G8B8A8_SINT;
		D3D11_SUBRESOURCE_DATA initData = {};
		D3D11_TEXTURE2D_DESC desc = {};

		initData.pSysMem = src;
		initData.SysMemPitch = width * sizeof(src);
		initData.SysMemSlicePitch = initData.SysMemPitch * height;

		desc.Width = width;
		desc.Height = height;
		desc.MipLevels = 1;
		desc.ArraySize = 1;
		desc.Format = format;
		desc.SampleDesc.Count = 1;
		desc.Usage = D3D11_USAGE_IMMUTABLE;
		desc.BindFlags = D3D11_BIND_SHADER_RESOURCE;

		ID3D11Texture2D* tex;

		HRESULT hr = device->CreateTexture2D(&desc, &initData, &tex);

		if (SUCCEEDED(hr))
		{
			D3D11_SHADER_RESOURCE_VIEW_DESC SRVDesc = {};

			SRVDesc.Format = format;
			SRVDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
			SRVDesc.Texture2D.MipLevels = 1;

			hr = device->CreateShaderResourceView(tex,
				&SRVDesc, &textures[textureSlot]);
		}

		if (FAILED(hr))
		{
			return CREATE_TEXTURE_ERROR;
		}


		return NO_ERROR_CODE;
	}

	int DXUpdateVertexBuffer(int verticesCount, Vertex* vertices)
	{
		int result = ChangeVertexBufferSize(verticesCount);

		if (result != NO_ERROR_CODE)
		{
			return result;
		}

		D3D11_MAPPED_SUBRESOURCE mapped_vertices = {};

		deviceContext->Map(g_pVertexBuffer, 0, D3D11_MAP_WRITE_DISCARD, 0, &mapped_vertices);
		memcpy(mapped_vertices.pData, vertices, verticesCount * sizeof(Vertex));
		deviceContext->Unmap(g_pVertexBuffer, 0);

		return NO_ERROR_CODE;
	}

	void ClearBackground(float r, float g, float b)
	{
		float ClearColor[4] = { r, g, b, 1.0f }; // red,green,blue,alpha
		deviceContext->ClearRenderTargetView(g_pRenderTargetView, ClearColor);
	}

	void DXDraw(int startIndex, int verticesCount, UINT textureSlot)
	{
		deviceContext->PSSetShaderResources(0, 1, &textures[textureSlot]);

		deviceContext->Draw(verticesCount, startIndex);

		swapChain->Present(0, 0);
	}

	int ChangeVertexBufferSize(int size)
	{
		if (size > vertext_buffer_size)
		{
			if (sys_vertices) delete[] sys_vertices;

			sys_vertices = new Vertex[size];

			D3D11_BUFFER_DESC bd = {};
			D3D11_SUBRESOURCE_DATA InitData = {};

			bd.Usage = D3D11_USAGE_DYNAMIC;
			bd.ByteWidth = sizeof(Vertex) * size;
			bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
			bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;

			InitData.pSysMem = sys_vertices;

			HRESULT hr = device->CreateBuffer(&bd, &InitData, &g_pVertexBuffer);

			if (FAILED(hr))
			{
				return CREATE_VERTEX_BUFFER_ERROR;
			}

			UINT offset = 0;
			UINT stride = sizeof(Vertex);

			deviceContext->IASetVertexBuffers(0, 1, &g_pVertexBuffer, &stride, &offset);

			vertext_buffer_size = size;
		}

		return NO_ERROR_CODE;
	}

	void DXRelease()
	{	
		if (sys_vertices) delete[] sys_vertices;
		if (deviceContext) deviceContext->ClearState();

		//TODO:Memmory allocation not implement
		/*std::for_each(textures.begin(), textures.end(), [](auto texture)
			{
				((ID3D11ShaderResourceView*)texture)->Release();
			});*/

		if (samplerState) samplerState->Release();
		if (g_pVertexBuffer) g_pVertexBuffer->Release();
		if (g_pVertexLayout) g_pVertexLayout->Release();
		if (g_pVertexShader) g_pVertexShader->Release();
		if (g_pPixelShader) g_pPixelShader->Release();
		if (g_pRenderTargetView) g_pRenderTargetView->Release();
		if (swapChain) swapChain->Release();
		if (deviceContext) deviceContext->Release();
		if (device) device->Release();
	}
};