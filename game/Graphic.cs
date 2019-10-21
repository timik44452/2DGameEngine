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
    private uint[] loaded_sprites;

    private RendererQueue rendererQueue;
    private Matrix viewMatrix;

    private Graphic(int width, int height)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        loaded_sprites = new uint[0];
        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];

        viewMatrix = new Matrix(
            100F / width, 0,
            0, 100F / height);

        rendererQueue = new RendererQueue();

        graphics = this;
    }

    public static Graphic Create(int width, int height, IntPtr handle)
    {
        var pixelShader = Resourcepack.GetResource<Shader>("pixelShader");
        var vertexShader = Resourcepack.GetResource<Shader>("vertexShader");

        GameDebug.DXLog(CIntegrations.InitDevice(width, height, handle));
        GameDebug.DXLog(CIntegrations.LoadShaders(pixelShader.path, vertexShader.path));

        return new Graphic(width, height);
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        old_frame_time = DateTime.Now;

        rendererQueue.CheckVertexBuffer(gameObjects.Length * 6);

        for (int i = 0; i < gameObjects.Length; i++)
            DrawGameObject(camera, gameObjects[i], i);

        CIntegrations.UpdateVertexBuffer(rendererQueue.VertexBuffer);

        new_frame_time = DateTime.Now;
    }

    public void Draw()
    {
        CIntegrations.ClearBackground(ColorAtlas.Black);

        foreach (RendererQueueItem item in rendererQueue.RendererItems)
            CIntegrations.Draw(item.VertexIndex, item.VertexCount, item.sprite);

        rendererQueue.ResetQueueIndex();
    }

    private void DrawGameObject(Camera camera, GameObject gameObject, int i)
    {
        i *= 6;

        gameObject.Update();

        Sprite sprite = gameObject.renderer.sprite;

        if (sprite != null && camera.viewport.Contain(gameObject.transform.position))
        {
            Vector delta = gameObject.transform.scale * 0.5f;
            Vector worldTransform = gameObject.transform.position - camera.gameObject.transform.position;

            rendererQueue.SetVertexBufferItem(i, gameObject.Layer, sprite,
                Matrix.Multiply(viewMatrix, worldTransform - delta),
                Matrix.Multiply(viewMatrix, worldTransform + delta));

            for (int l = 0; l < loaded_sprites.Length; l++)
                if (loaded_sprites[l] == sprite.UID)
                    return;

            GameDebug.DXLog(CIntegrations.CreateDXResource(sprite));

            var temp = loaded_sprites;
            loaded_sprites = new uint[temp.Length + 1];
            temp.CopyTo(loaded_sprites, 0);
            loaded_sprites[temp.Length] = sprite.UID;
        }
    }
    
}