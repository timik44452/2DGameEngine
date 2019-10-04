#pragma once
#include<d3dcompiler.h>

struct Vertex
{
	float x, y, z;
};

enum ERROR_CODE
{
	PIXEL_SHADER_COMPILE_ERROR = 0x00101,
	VERTEX_SHADER_COMPILE_ERROR = 0x00102,
	PIXEL_SHADER_LOADING_ERROR = 0x00111,
	VERTEX_SHADER_LOADING_ERROR = 0x00112,

	NO_ERROR_CODE = 0x00055
};

class DirectXCore
{
public:
	ID3D11Buffer* vertexBuffer;

	ID3D11Device* device;
	ID3D11DeviceContext* deviceContext;
	ID3D11RenderTargetView* renderTargetView;

	ID3D11InputLayout* inputLayout;
	ID3D11PixelShader* pixelShader;
	ID3D11VertexShader* vertexShader;

	IDXGISwapChain* swapChain;

	int DXInitDevice(int Width, int Height, HWND context)
	{
		//UINT flags = 0;
		DXGI_SWAP_CHAIN_DESC swapChainDesc;

		// ������� ��������� � �������� ���������� ����� ����
		swapChainDesc.BufferDesc.Width = Width;
		swapChainDesc.BufferDesc.Height = Height;
		// ����������� ���������� ������ � ������� �������� � ���� ������������� �����
		// �.�. ��� ����� ������������ ������� ������, ���������
		swapChainDesc.BufferDesc.RefreshRate.Numerator = 0;
		swapChainDesc.BufferDesc.RefreshRate.Denominator = 1;
		// ������ ������ -- 32-������ RGBA
		swapChainDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		// �� ������ ��������������� ��� ������
		swapChainDesc.BufferDesc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED;
		swapChainDesc.BufferDesc.Scaling = DXGI_MODE_SCALING_UNSPECIFIED;
		// �� ���������� �����������
		swapChainDesc.SampleDesc.Count = 1;
		swapChainDesc.SampleDesc.Quality = 0;
		// ���������� SwapChain ��� ������
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		// ���� "������" (�� ������������) �����
		swapChainDesc.BufferCount = 1;
		// ������ ���� ��� ������
		swapChainDesc.OutputWindow = context;
		// ������� �����
		swapChainDesc.Windowed = TRUE;
		// ����������� ������ ���������� �� ������ ��� ������ �� �����
		swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
		swapChainDesc.Flags = 0;


		// ���������� DirectX 11.0, �.�. ��� ��� ����������
		D3D_FEATURE_LEVEL featureLevel = D3D_FEATURE_LEVEL::D3D_FEATURE_LEVEL_11_0;// D3D_FEATURE_LEVEL_11_0;

		HRESULT result = D3D11CreateDeviceAndSwapChain(
			// ���������� ������������ ��-���������
			NULL,
			// ���������� ���������� ����������
			D3D_DRIVER_TYPE_HARDWARE, NULL,
			// ��. ����
			0,
			// ���������� ���� ������ DirectX
			&featureLevel, 1,
			// ������ SDK
			D3D11_SDK_VERSION,
			// �������� ��������� ����� ��������
			&swapChainDesc,
			// ���������, ���� �������� ���������
			&swapChain, &device, NULL, &deviceContext);

		assert(SUCCEEDED(result));

		Vertex vertices[]
		{
			{ -1.0f, -1.0f, 0.0f },
			{ 1.0f, 1.0f, 0.0f },
			{ 1.0f, -1.0f, 0.0f },
		};

		D3D11_BUFFER_DESC bd = { 0 };

		bd.ByteWidth = sizeof(Vertex) * 3;
		bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;

		D3D11_SUBRESOURCE_DATA srd = { vertices, 0, 0 };

		device->CreateBuffer(&bd, &srd, &vertexBuffer);

		return NO_ERROR_CODE;
	}

	int DXSetViewport(int Width, int Height)
	{
		CD3D11_VIEWPORT viewport;

		viewport.TopLeftX = 0;
		viewport.TopLeftY = 0;

		viewport.Width = Width;
		viewport.Height = Height;

		viewport.MinDepth = 0;
		viewport.MaxDepth = 1;

		HRESULT result;
		ID3D11Texture2D* backBuffer;

		// ����� "������" ����� �� SwapChain
		result = swapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (void**)&backBuffer);
		assert(SUCCEEDED(result));

		// �������������� ������ � ������ ��� ������ � ��� ���������
		result = device->CreateRenderTargetView(backBuffer, NULL, &renderTargetView);
		assert(SUCCEEDED(result));

		// ��������� �� ����� ������ ��� �� �����
		// ����� ��������, ��� ��� ����� ��� ���� �� ���������,
		// �.�. �� ���� �� ��� ��������� SwapChain,
		// Release() ���� ����������� ���������
		backBuffer->Release();

		// ���������� ��������� View ��� ���������
		deviceContext->OMSetRenderTargets(1, &renderTargetView, NULL);
		deviceContext->RSSetViewports(1, &viewport);

		return NO_ERROR_CODE;
	}

	int DXLoadShaders(
		const char* pixelShaderPath,
		const char* vertexShaderPath)
	{
		HRESULT result;
		ID3DBlob* blob = NULL;
		
		result = D3DCompileFromFile(L"C:\pixelShader.hlsl", NULL, NULL, "main", "px_5_0", 0, 0, &blob, NULL);
		
		if (FAILED(result))
		{
			return PIXEL_SHADER_COMPILE_ERROR;
		}

		result = D3DCompileFromFile((LPCWSTR)vertexShaderPath, NULL, NULL, "main", "vx_5_0", 0, 0, &blob, NULL);

		if (FAILED(result))
		{
			return VERTEX_SHADER_COMPILE_ERROR;
		}
		
		result = device->CreatePixelShader(blob->GetBufferPointer(), blob->GetBufferSize(), NULL, &pixelShader);

		if (FAILED(result))
		{
			return PIXEL_SHADER_LOADING_ERROR;
		}

		result = device->CreateVertexShader(blob->GetBufferPointer(), blob->GetBufferSize(), NULL, &vertexShader);

		if (FAILED(result))
		{
			return VERTEX_SHADER_LOADING_ERROR;
		}

		// ������, ��� � ��������� ������ ����� ��������� ������
		// �������� ������� (� �������������) ��������� �������
		D3D11_INPUT_ELEMENT_DESC inputDesc;
		// ������������� ��� ���������
		inputDesc.SemanticName = "POSITION";
		// ����� ������ � ������, ���� ��������� � ������ ������������� ������ ������ ������
		inputDesc.SemanticIndex = 0;
		// ��������� ������ �� 32-������ ������������ �����
		inputDesc.Format = DXGI_FORMAT_R32G32_FLOAT;
		// �������������� ��������
		inputDesc.AlignedByteOffset = D3D11_APPEND_ALIGNED_ELEMENT;
		// ��� ������ �������
		inputDesc.InputSlotClass = D3D11_INPUT_PER_VERTEX_DATA;
		// ������ ��������
		inputDesc.InputSlot = 0;
		// ���������� ������� ��� ���������
		inputDesc.InstanceDataStepRate = 0;

		result = device->CreateInputLayout(
			// ������ �������� ���������� � ��� �����
			&inputDesc, 1,
			// ������� � ��� �����
			blob->GetBufferPointer(), blob->GetBufferSize(),
			// ��������� �����
			&inputLayout);
		assert(SUCCEEDED(result));

		// ������������� ������������ �������
		// ���������
		deviceContext->VSSetShader(vertexShader, NULL, 0);
		// ����������
		deviceContext->PSSetShader(pixelShader, NULL, 0);

		// ������������� ������ � ������ ��������� � ��������������� �������
		// deviceContext->CSSetUnorderedAccessViews(1, 1, &velocityUAV, NULL);
		// ������������� ������ ������ ���������� ���������� �������
		deviceContext->IASetInputLayout(inputLayout);
		//// �������� ����� �����
		deviceContext->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_POINTLIST);

		return NO_ERROR_CODE;
	}

	void DXDraw(int verticesCount, Vertex* vertices)
	{
		// ������� ����� ������ ������
		float clearColor[] = { 0.0f, 0.0f, 0.0f, 0.0f };
		deviceContext->ClearRenderTargetView(renderTargetView, clearColor);

		// ��������� ������� �� 32-������ ������������ ���� ������
		UINT stride = sizeof(Vertex);
		UINT offset = 0;

		ID3D11Buffer* nullBuffer = NULL;
		ID3D11UnorderedAccessView* nullUAV = NULL;


		deviceContext->IASetVertexBuffers(0, 1, &vertexBuffer, &stride, &offset);

		deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		// �������� ���������
		deviceContext->Draw(3, 0);

		// ������� ����������� �� �����
		swapChain->Present(1, 0);
	}

	void DXRelease()
	{
		DXReleaseBuffer();
		DXReleaseDevice();
		DXReleaseShaders();
		DXReleaseRenderTarget();
	}
private :
	void DXReleaseBuffer()
	{
		vertexBuffer->Release();
	}

	void DXReleaseDevice()
	{
		device->Release();
		deviceContext->Release();
		swapChain->Release();
	}

	void DXReleaseShaders()
	{
		pixelShader->Release();
		vertexShader->Release();
		inputLayout->Release();
	}

	void DXReleaseRenderTarget()
	{
		renderTargetView->Release();
	}
};