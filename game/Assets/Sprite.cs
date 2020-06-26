using System;
using System.Runtime.InteropServices;

public unsafe class Sprite : Asset, IDisposable
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private IntPtr ptr;
    private IntPtr tempPtr;

    public Sprite(Texture texture)
    {
        Width = texture.Width;
        Height = texture.Height;

        int Length = Width * Height;
        int size = Marshal.SizeOf(typeof(Color));
        tempPtr = Marshal.AllocHGlobal(Length * size);
        ptr = Marshal.AllocHGlobal(Length * size);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SetColor(x, y, texture.GetColor(x / (float)Width, y / (float)Height));
            }
        }
    }

    public IntPtr GetIntPtr()
    {
        return ptr;
    }

    public void Dispose()
    {
        Marshal.Release(tempPtr);
        Marshal.Release(ptr);
    }

    private void SetColor(int x, int y, Color color)
    {
        int index = x + y * Width;
        
        if (index >= 0 && index < Width * Height)
        {
            Marshal.StructureToPtr(color, tempPtr, true);

            Marshal.WriteIntPtr(ptr, index * Marshal.SizeOf(color), tempPtr);
        }
    }
}