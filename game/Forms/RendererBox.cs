using System;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class RendererBox : UserControl, IControlBehaviour
{
    public float deltaTime { get; private set; }

    private const int fixed_fps = 60;

    private World map;
    private InputContext inputContext;


    public RendererBox()
    {
        InitializeComponent();
    }

    public void OnClosed(object sender)
    {
        GraphicsAPI.currentGraphic.Release();
    }

    public void OnLoad(object sender)
    {
        InputHandlerInitialize();

        Resourcepack.Loadresources();

        Service.Tiling.LoadTilesFromMap("tilemap",
           new TilemapCell("grass", 640, 0, 32, 32),
           new TilemapCell("tree", 384, 448, 64, 64),
           new TilemapCell("wall", 384, 320, 32, 32),
           new TilemapCell("floor", 0, 640, 32, 32));

        map = new World();
        inputContext = new InputContext();

        var graphics = CreateGraphics();

        Input.Initialize(inputContext);
        GraphicsAPI.Initialize(Width, Height, new System.Runtime.InteropServices.HandleRef(graphics, graphics.GetHdc()));
    }

    public void OnPaint(object sender)
    {
        //if(deltaTime < 1000 / fixed_fps)
        //{
        //    deltaTime -= 0.001F;
        //    return;
        //}

        DateTime time = DateTime.Now;

        foreach (GameObject gameObject in map.GetAllObjects())
            gameObject.Update();

        if (map.CameraCount > 0)
        {
            foreach (GameObject cameraGameObject in map.GetCameraObjects())
            {
                GraphicsAPI.currentGraphic.BeginRenderer(cameraGameObject.camera);

                Parallel.ForEach(map.GetDrawbleObjects(), drawbleGameObject =>
                {
                    GraphicsAPI.currentGraphic.Draw(
                        drawbleGameObject.transform.position,
                        drawbleGameObject.transform.scale,
                        drawbleGameObject.renderer.sprite,
                        drawbleGameObject.transform.space == Space.World);
                });
            }

            GraphicsAPI.currentGraphic.EndRenderer();
        }

        deltaTime = (float)(DateTime.Now - time).TotalMilliseconds;
        inputContext.Update();
    }

    private void InputHandlerInitialize()
    {
        MouseMove += (object sender, MouseEventArgs e) =>
        {
            inputContext.mousePosition.X = e.X;
            inputContext.mousePosition.Y = e.Y;
        };

        KeyDown += (object sender, KeyEventArgs e) => inputContext.KeyDown(e.KeyCode.ToString());
        KeyUp += (object sender, KeyEventArgs e) => inputContext.KeyUp(e.KeyCode.ToString());
    }
}