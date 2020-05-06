public class TilemapCell
{
    public string name;

    public int X;
    public int Y;
    public int Width;
    public int Height;

    public TilemapCell(string name, int X, int Y, int Width, int Height)
    {
        this.name = name;
        this.X = X;
        this.Y = Y;
        this.Height = Height;
        this.Width = Width;
    }
}
