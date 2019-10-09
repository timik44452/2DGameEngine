using System.Drawing;
using System.Drawing.Imaging;

public class Texture : Asset
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Color[] colors;

    public Texture(string path)
    {
        Load(path);
    }

    public Texture(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;

        colors = new Color[Width * Height];
    }

    public Color GetPixel(int x, int y)
    {
        int index = x + y * Width;

        if (index >= 0 && index < colors.Length)
            return colors[index];
        else
            return ColorAtlas.Transparent;
    }

    public void SetPixel(int x, int y, Color color)
    {
        int index = x + y * Width;

        if (index >= 0 && index < colors.Length)
            colors[index] = color;

    }

    public Color GetPixel(int index)
    {
        if (index >= 0 && index < colors.Length)
            return colors[index];
        else
            return ColorAtlas.Transparent;
    }

    public Color GetColor(float u, float v)
    {
        int x = (int)(u * Width);
        int y = (int)(v * Height);

        return GetPixel(x, y);
    }

    public void Load(string fileName)
    {
        if (System.IO.File.Exists(fileName))
        {
            Bitmap bitmap = Image.FromFile(fileName) as Bitmap;

            Width = bitmap.Width;
            Height = bitmap.Height;
            colors = new Color[Width * Height];

            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);

                    colors[x + y * Width] = ColorHelper.GetColor(color.R, color.G, color.B, color.A);
                }
        }
    }
}