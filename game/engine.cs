using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class engine : Form
    {
        private const bool IsFullWindow = true;
        private const int unitSize = 32;


        private World map;
        private Input input;
        private Camera camera;
        
        public engine()
        {
            InitializeComponent();

            if (IsFullWindow)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            Resourcepack.Loadresources();
            Resourcepack.LoadTilemap(new TilemapCell("grass", 640, 0, 32, 32));

            map = new World();
            input = new Input();
        }

        private void PaintScene()
        { 
            Graphic.graphics.Clear();
            Graphic.graphics.DrawGameObjects(camera, map.GetViewedObjects());
            Graphic.graphics.GDIDraw();

            Input.WorldMousePosition = new Vector(MousePosition.X % Width, MousePosition.Y / Width);

            Text = $"{1000 / Time.deltaTime} ms";
        }

        private void Engine_Load(object sender, EventArgs e)
        {
            camera = new Camera(new Rect(-unitSize, -unitSize, Width + unitSize, Height + unitSize));

            Graphic.Create(Width, Height, CreateGraphics());

            var timer1 = new Timer();

            timer1.Tick += (object s, EventArgs args) => PaintScene();

            timer1.Interval = 1;

            timer1.Start();
        }
    }
}
