using System;
using System.Drawing;

public class Sprite
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private CIntegrations.Color[] colors;

    public Sprite(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;

        colors = new CIntegrations.Color[Width * Height];
    }

    public Sprite(Texture texture)
    {
        Width = texture.Width;
        Height = texture.Height;

        colors = new CIntegrations.Color[Width * Height];

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Color color = texture.GetColor(x / (float)Width, y / (float)Height);

                colors[x + y * Width] = new CIntegrations.Color(color.R, color.G, color.B, color.A, color.rgb);
            }
    }

    public void SetColor(int x, int y, Color color)
    {
        int index = x + y * Width;

        if(index >= 0 && index < colors.Length)
        {
            colors[x + y * Width] = new CIntegrations.Color(color.R, color.G, color.B, color.A, color.rgb);
        }
    }

    public CIntegrations.Color[] GetTColors()
    {
        return colors;
    }
}