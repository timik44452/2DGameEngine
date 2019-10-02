public struct Rect
{
    private float X0;
    private float Y0;

    public float X { get; private set; }
    public float Y { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }

    public static Rect Empty { get => new Rect(0,0,0,0); }

    public Rect(Vector position, Vector size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.X;
        Height = size.Y;

        X0 = X + Width;
        Y0 = Y + Height;
    }

    public Rect(float X, float Y, float Width, float Height)
    {
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;

        X0 = X + Width;
        Y0 = Y + Height;
    }

    public void SetX(float value)
    {
        X = value;
    }

    public void SetY(float value)
    {
        Y = value;
    }
    public void SetWidth(float value)
    {
        Width = value;
        Update();
    }
    public void SetHeight(float value)
    {
        Height = value;
        Update();
    }
    public void Set(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;

        Update();
    }

    public bool Contain(Rect rect)
    {
        return
            (rect.X < X0 ||
            rect.X0 < X) &&
            (rect.Y < Y0 ||
            rect.Y0 < Y);
    }
    public bool Contain(float x, float y)
    {
        return
            x >= X && x <= X0 &&
            y >= Y && y <= Y0;
    }

    public bool Contain(Vector point)
    {
        return Contain(point.X, point.Y);
    }

    public bool Contain(float x, float y, float delta)
    {
        return
            x >= X - delta && x <= X0 + delta &&
            y >= Y - delta && y <= Y0 + delta;
    }

    private void Update()
    {
        X0 = X + Width;
        Y0 = Y + Height;
    }
}