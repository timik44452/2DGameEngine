using System;

public class DirectXGraphic : IGraphic
{
    public DateTime old_frame_time;
    public DateTime new_frame_time;

    private int width;
    private int height;
    private int bufferSize;

    private int[] data;
    private int[] layersBuffer;
    private int[] loaded_sprites;

    private RendererQueue rendererQueue;
    private Matrix2x2 viewMatrix;

    private DirectXGraphic(int width, int height)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        loaded_sprites = new int[0];
        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];

        viewMatrix = new Matrix2x2(
            100F / width, 0,
            0, 100F / height);

        rendererQueue = new RendererQueue();
    }

    public static DirectXGraphic Create(int width, int height, IntPtr handle)
    {
        var pixelShader = Resourcepack.GetResource<Shader>("pixelShader");
        var vertexShader = Resourcepack.GetResource<Shader>("vertexShader");

        GameDebug.DXLog(CIntegrations.InitDevice(width, height, handle));
        GameDebug.DXLog(CIntegrations.LoadShaders(pixelShader.path, vertexShader.path));

        return new DirectXGraphic(width, height);
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
                Matrix2x2.Multiply(viewMatrix, worldTransform - delta),
                Matrix2x2.Multiply(viewMatrix, worldTransform + delta));

            for (int l = 0; l < loaded_sprites.Length; l++)
                if (loaded_sprites[l] == sprite.UID)
                    return;

            GameDebug.DXLog(CIntegrations.CreateDXResource(sprite));

            var temp = loaded_sprites;
            loaded_sprites = new int[temp.Length + 1];
            temp.CopyTo(loaded_sprites, 0);
            loaded_sprites[temp.Length] = sprite.UID;
        }
    }

    public void Release()
    {
        CIntegrations.Release();
    }
}