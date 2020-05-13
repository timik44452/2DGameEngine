using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class RendererBox : UserControl, IControlBehaviour
    {
        public float deltaTime { get; private set; }

        private const int fixed_fps = 60;

        private World map;
        private Input input;
        

        public RendererBox()
        {
            InitializeComponent();
        }

        public void OnClosed(object sender)
        {
            GraphicCore.currentGraphic.Release();
        }

        public void OnLoad(object sender)
        {
            Resourcepack.Loadresources();

            Service.Tiling.LoadTilesFromMap("tilemap",
               new TilemapCell("grass", 640, 0, 32, 32),
               new TilemapCell("tree", 384, 448, 64, 64),
               new TilemapCell("wx", 0, 0, 32, 32));

            map = new World();
            input = new Input();

            var graphics = CreateGraphics();

            GraphicCore.Initialize(Width, Height, new System.Runtime.InteropServices.HandleRef(graphics, graphics.GetHdc()));
        }

        public void OnPaint(object sender)
        {
            DateTime time = DateTime.Now;

            if (map.GetCameraObject() != null)
                map.GetCameraObject().camera.Renderer();

            GraphicCore.currentGraphic.Draw();

            deltaTime = (float)(DateTime.Now - time).TotalMilliseconds;
        }
    }
}
