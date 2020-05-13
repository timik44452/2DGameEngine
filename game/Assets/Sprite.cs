using System;

public class Sprite : Asset
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Color[] colors;

    public int[] Buffer;

    public Sprite(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;

        colors = new Color[Width * Height];
        Buffer = new int[Width * Height];
    }

    public Sprite(Texture texture)
    {
        Width = texture.Width;
        Height = texture.Height;

        colors = new Color[Width * Height];
        Buffer = new int[Width * Height];

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                SetColor(x , y, texture.GetColor(x / (float)Width, y / (float)Height));  
            }
    }

    public void Apply()
    {
        GameDebug.DXLog(CIntegrations.CreateDXResource(this));
    }

    public void SetColor(int x, int y, Color color)
    {
        int index = x + y * Width;

        if (index >= 0 && index < colors.Length)
        {
            Buffer[index] = color.GetRInt32() << 16 | color.GetGInt32() << 8 | color.GetBInt32();

            colors[index] = color;
        }
    }

    public void Save(string path)
    {
        Service.ImageProcessor.Save(path, Buffer, Width, Height);
    }
}