using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class engine : Form
    {
        private const bool IsFullWindow = true;
        private const int fixed_fps = 60;

        private World map;
        private Input input;
        
        public engine()
        {
            InitializeComponent();

            if (IsFullWindow)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            Resourcepack.Loadresources();
            Service.Tiling.LoadTilesFromMap("tilemap", new TilemapCell("grass", 640, 0, 32, 32));

            map = new World();
            input = new Input();
        }

        private void Engine_Load(object sender, EventArgs e)
        {
            Graphic.Create(Width, Height, Handle);

            Timer timer = new Timer();
            timer.Interval = 1;
            timer.Tick += PaintScene;
            timer.Start();
        }

        private void PaintScene(object sender, EventArgs e)
        {
            if (map.GetCameraObject() != null)
                map.GetCameraObject().camera.Renderer();

            Graphic.graphics.Draw();

            Text = $"{1000 / Time.deltaTime} FPS";
        }

        private void engine_FormClosing(object sender, FormClosingEventArgs e)
        {
            CIntegrations.Release();
        }
    }
}