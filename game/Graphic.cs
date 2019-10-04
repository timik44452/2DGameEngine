using System;
using System.Runtime.InteropServices;
using System.Drawing;


public class Graphic
{
    public static Graphic graphics { get; private set; }

    private int width;
    private int height;
    private int bufferSize;

    private int[] data;
    private int[] layersBuffer;

    private CIntegrations.Vertex[] vertices = new CIntegrations.Vertex[3];


    private Graphic(int width, int height)
    {
        this.width = width;
        this.height = height;

        bufferSize = width * height;

        layersBuffer = new int[bufferSize];
        data = new int[bufferSize];

        graphics = this;

        graphics.vertices[0].x = -1;
        graphics.vertices[0].y = -1;

        graphics.vertices[1].x = 1;
        graphics.vertices[1].y = 1;

        graphics.vertices[2].x = 1;
        graphics.vertices[2].y = -1;

        

        
        

    }

    public static Graphic Create(int width, int height, IntPtr handle)
    {
        var pixelShader = Resourcepack.GetResource<Shader>("pixelShader");
        var vertexShader = Resourcepack.GetResource<Shader>("vertexShader");

        CIntegrations.MessageDevice(pixelShader.path, handle);

        CIntegrations.InitDevice(width, height, handle);
        CIntegrations.SetViewport(width, height);
        int code = CIntegrations.LoadShaders(ref pixelShader.path.ToCharArray()[0], ref vertexShader.path.ToCharArray()[0]);

        System.Windows.Forms.MessageBox.Show(code.ToString());

        return new Graphic(width, height);
    }

    public void DrawGameObjects(Camera camera, GameObject[] gameObjects)
    {
        DateTime now = DateTime.Now;

        //for (int i = 0; i < gameObjects.Length; i++)
        //   DrawGameObject(camera, gameObjects[i]);

        //System.Threading.Tasks.Parallel.For(0, gameObjects.Length, i => DrawGameObject(camera, gameObjects[i]));

        CIntegrations.Draw(3, ref vertices[0]);

        Time.deltaTime = (float)(DateTime.Now - now).TotalMilliseconds;
    }

    private void DrawGameObject(Camera camera, GameObject gameObject)
    {
        int posX = (int)((gameObject.transform.position.X - camera.position.X) * 32);
        int posY = (int)((gameObject.transform.position.Y - camera.position.Y) * 32);

        gameObject.Update();

        int delta = posX + posY * width;

        Sprite sprite = gameObject.renderer.sprite;

        if (camera.viewport.Contain(posX, posY) && sprite != null)
        {
            //TODO:HERE GAMEOBJECT DRAWING
        }
    }
}

