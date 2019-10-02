using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WindowsFormsApp5
{
    public partial class engine : Form
    {
        private const bool IsFullWindow = true;
        private const int unitSize = 32;


        private World map;
        private Input input;
        private Audio audio;
        private Camera camera;
        private LightMap lightMap;
        private CollisionManager collisionManager;

        private Graphic graphic;

        private float renderTime;
        private float fixedTimer;

        public engine()
        {
            InitializeComponent();

            if (IsFullWindow)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            LoadResources();

            map = new World();
            input = new Input();
            audio = new Audio();
            collisionManager = new CollisionManager();
        }

        private void PaintScene()
        { 
            DateTime now = DateTime.Now;

            Graphic.graphics.Clear();

            for (int i = 0; i < map.GetViewedObjects().Length; i++)
                DrawGameObject(map.GetViewedObjects()[i]);

            graphic.Draw();

            renderTime = Math.Max(1, (DateTime.Now - now).Milliseconds);
            fixedTimer += renderTime;
            Text = $"FPS:{1000 / renderTime}";

            if (fixedTimer >= 5)
            {
                fixedTimer = 0;

                for (int i = 0; i < map.GetAllObjects().Length; i++)
                    map.GetAllObjects()[i].FixedUpdate();
            }

            Input.WorldMousePosition = new Vector(MousePosition.X % Width, MousePosition.Y / Width);
        }

        private void DrawGameObject(GameObject gameObject)
        {
            int posX = (int)((gameObject.transform.position.X - camera.position.X) * unitSize);
            int posY = (int)((gameObject.transform.position.Y - camera.position.Y) * unitSize);

            gameObject.Update();

            int delta = posX + posY * Width;

            Sprite sprite = gameObject.renderer.sprite;

            if (camera.viewport.Contain(posX, posY) && sprite != null)
            {
                gameObject.OnDraw();

                Graphic.graphics.DrawGameObject(delta, 
                    gameObject.Layer, 
                    sprite.Width * gameObject.transform.HorizontalOrientation, 
                    sprite.Height * gameObject.transform.VerticalOrientation, 
                    sprite.GetTColors());
            }
        }

        private void Engine_FormClosing(object sender, FormClosingEventArgs e)
        {
            //drawingThread?.Abort();
        }

        private void LoadResources()
        {
            Resourcepack.Loadresources();
        }

        private void Engine_Load(object sender, EventArgs e)
        {
            lightMap = new LightMap(Width, Height);
            camera = new Camera(new Rect(-unitSize, -unitSize, Width + unitSize, Height + unitSize));

            var graphics = CreateGraphics();
            graphic = new Graphic(Width, Height, new HandleRef(graphics, graphics.GetHdc()));
            Graphic.graphics = graphic;

            var timer1 = new Timer();

            timer1.Tick += (object s, EventArgs args) => PaintScene();

            timer1.Interval = 1;

            timer1.Start();
        }
    }
}
