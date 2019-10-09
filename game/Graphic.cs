using System;
using System.Runtime.InteropServices;
using System.Drawing;


public class Graphic
{
    public static Graphic graphics { get; private set; }

    public DateTime old_frame_time;
    public DateTime new_frame_time;

    private int width;
    private int height;
    private int bufferSize;

    private int[] data;
    private int[] layersBuffer;

    private Vertex[] vertices = new Vertex[6];
    private Matrix viewMatrix;

    private Graphic(int width, int height)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];

        viewMatrix = new Matrix(
            100F / width, 0,
            0, 100F / height);

        graphics = this;
    }

    public static Graphic Create(int width, int height, IntPtr handle)
    {
        var pixelShader = Resourcepack.GetResource<Shader>("pixelShader");
        var vertexShader = Resourcepack.GetResource<Shader>("vertexShader");

        var sprite = Resourcepack.GetResource<Sprite>("grass");

        GameDebug.DXLog(CIntegrations.InitDevice(width, height, handle));

        GameDebug.DXLog(CIntegrations.LoadTextureFromInt(ref sprite.Buffer[0], sprite.Width, sprite.Height));
        GameDebug.DXLog(CIntegrations.LoadShaders(pixelShader.path, vertexShader.path));

        return new Graphic(width, height);
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        old_frame_time = DateTime.Now;

        int vertexLength = gameObjects.Length * 6;

        if (vertexLength > vertices.Length)
            ResizeVertexBuffer(vertexLength);
        
        for (int i = 0; i < gameObjects.Length; i++)
            DrawGameObject(camera, gameObjects[i], i);
            
        CIntegrations.UpdateBuffer(vertexLength, ref vertices[0]);

        //old_frame_time = new_frame_time;
        new_frame_time = DateTime.Now;
    }

    public void Draw()
    {
        CIntegrations.Draw(vertices.Length);
    }

    private void DrawGameObject(Camera camera, GameObject gameObject, int i)
    {
        i *= 6;

        gameObject.Update();

        Sprite sprite = gameObject.renderer.sprite;

        if (sprite != null && camera.viewport.Contain(gameObject.transform.position))
        {
            //      p1
            // d----b
            // |   /|
            // |  / |
            // | /  |
            // |/   |
            // a ---c
            //p0

            Vector delta = gameObject.transform.scale * 0.5f;
            Vector worldTransform = gameObject.transform.position - camera.gameObject.transform.position;

            Vector point0 = Matrix.Multiply(viewMatrix, worldTransform - delta);
            Vector point1 = Matrix.Multiply(viewMatrix, worldTransform + delta);

            vertices[i].x = point0.X;
            vertices[i].y = point0.Y;

            vertices[i + 1].x = point1.X;
            vertices[i + 1].y = point1.Y;

            vertices[i + 2].x = point1.X;
            vertices[i + 2].y = point0.Y;


            vertices[i + 3].x = point0.X;
            vertices[i + 3].y = point0.Y;

            vertices[i + 4].x = point0.X;
            vertices[i + 4].y = point1.Y;

            vertices[i + 5].x = point1.X;
            vertices[i + 5].y = point1.Y;
        }
    }
    private void ResizeVertexBuffer(int size)
    {
        vertices = new Vertex[size];

        // d----b
        // |   /|
        // |  / |
        // | /  |
        // |/   |
        // a ---c

        for (int i = 0; i < size; i+=6)
        {
            //a
            vertices[i].u = 0;
            vertices[i].v = 1;

            //b
            vertices[i + 1].u = 1;
            vertices[i + 1].v = 0;

            //c
            vertices[i + 2].u = 1;
            vertices[i + 2].v = 1;

            //a
            vertices[i + 3].u = 0;
            vertices[i + 3].v = 1;

            //d
            vertices[i + 4].u = 0;
            vertices[i + 4].v = 0;

            //b
            vertices[i + 5].u = 1;
            vertices[i + 5].v = 0;
        }
    }
}