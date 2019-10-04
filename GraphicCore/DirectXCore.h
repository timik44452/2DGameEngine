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

		// Разрмер совпадает с размером клиентской части окна
		swapChainDesc.BufferDesc.Width = Width;
		swapChainDesc.BufferDesc.Height = Height;
		// Ограничение количества кадров в секунду задается в виде рационального числа
		// Т.к. нам нужна максимальная частота кадров, отключаем
		swapChainDesc.BufferDesc.RefreshRate.Numerator = 0;
		swapChainDesc.BufferDesc.RefreshRate.Denominator = 1;
		// Формат вывода -- 32-битный RGBA
		swapChainDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		// Не задаем масштабирования при выводе
		swapChainDesc.BufferDesc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED;
		swapChainDesc.BufferDesc.Scaling = DXGI_MODE_SCALING_UNSPECIFIED;
		// Не используем сглаживание
		swapChainDesc.SampleDesc.Count = 1;
		swapChainDesc.SampleDesc.Quality = 0;
		// Используем SwapChain для вывода
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		// Один "задний" (не отображаемый) буфер
		swapChainDesc.BufferCount = 1;
		// Задаем окно для вывода
		swapChainDesc.OutputWindow = context;
		// Оконный режим
		swapChainDesc.Windowed = TRUE;
		// Отбрасываем старую информацию из буфера при выводе на экран
		swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
		swapChainDesc.Flags = 0;


		// Используем DirectX 11.0, т.к. его нам достаточно
		D3D_FEATURE_LEVEL featureLevel = D3D_FEATURE_LEVEL::D3D_FEATURE_LEVEL_11_0;// D3D_FEATURE_LEVEL_11_0;

		HRESULT result = D3D11CreateDeviceAndSwapChain(
			// Используем видеоадаптер по-умолчанию
			NULL,
			// Используем аппаратную реализацию
			D3D_DRIVER_TYPE_HARDWARE, NULL,
			// См. выше
			0,
			// Используем одну версию DirectX
			&featureLevel, 1,
			// Версия SDK
			D3D11_SDK_VERSION,
			// Передаем созданное ранее описание
			&swapChainDesc,
			// Указатели, куда записать результат
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

		// Берем "задний" буфер из SwapChain
		result = swapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (void**)&backBuffer);
		assert(SUCCEEDED(result));

		// Инициализируем доступ к буферу для записи и для отрисовки
		result = device->CreateRenderTargetView(backBuffer, NULL, &renderTargetView);
		assert(SUCCEEDED(result));

		// Указатель на буфер больше нам не нужен
		// Стоит отметить, что сам буфер при этом не удаляется,
		// т.к. на него всё ещё указывает SwapChain,
		// Release() лишь освобождает указатель
		backBuffer->Release();

		// Используем созданный View для отрисовки
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

		// Задаем, как в вершинный шейдер будут вводиться данные
		// Описание первого (и единственного) аргумента функции
		D3D11_INPUT_ELEMENT_DESC inputDesc;
		// Семантическое имя аргумента
		inputDesc.SemanticName = "POSITION";
		// Нужно только в случае, если элементов с данным семантическим именем больше одного
		inputDesc.SemanticIndex = 0;
		// Двумерный вектор из 32-битных вещественных чисел
		inputDesc.Format = DXGI_FORMAT_R32G32_FLOAT;
		// Необязательный аргумент
		inputDesc.AlignedByteOffset = D3D11_APPEND_ALIGNED_ELEMENT;
		// Для каждой вершины
		inputDesc.InputSlotClass = D3D11_INPUT_PER_VERTEX_DATA;
		// Первый параметр
		inputDesc.InputSlot = 0;
		// Используем вершины для отрисовки
		inputDesc.InstanceDataStepRate = 0;

		result = device->CreateInputLayout(
			// Массив описаний аргументов и его длина
			&inputDesc, 1,
			// Байткод и его длина
			blob->GetBufferPointer(), blob->GetBufferSize(),
			// Структура ввода
			&inputLayout);
		assert(SUCCEEDED(result));

		// Устанавливаем используемые шейдеры
		// Вершинный
		deviceContext->VSSetShader(vertexShader, NULL, 0);
		// Пиксельный
		deviceContext->PSSetShader(pixelShader, NULL, 0);

		// Устанавливаем доступ к буферу скоростей у вычислительного шейдера
		// deviceContext->CSSetUnorderedAccessViews(1, 1, &velocityUAV, NULL);
		// Устанавливаем способ записи аргументов вершинного шейдера
		deviceContext->IASetInputLayout(inputLayout);
		//// Рисовать будем точки
		deviceContext->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_POINTLIST);

		return NO_ERROR_CODE;
	}

	void DXDraw(int verticesCount, Vertex* vertices)
	{
		// Очищаем буфер черным цветом
		float clearColor[] = { 0.0f, 0.0f, 0.0f, 0.0f };
		deviceContext->ClearRenderTargetView(renderTargetView, clearColor);

		// Двумерные вектора из 32-битных вещественных идут подряд
		UINT stride = sizeof(Vertex);
		UINT offset = 0;

		ID3D11Buffer* nullBuffer = NULL;
		ID3D11UnorderedAccessView* nullUAV = NULL;


		deviceContext->IASetVertexBuffers(0, 1, &vertexBuffer, &stride, &offset);

		deviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		// Вызываем отрисовку
		deviceContext->Draw(3, 0);

		// Выводим изображение на экран
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