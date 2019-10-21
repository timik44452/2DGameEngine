using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class RendererBox : UserControl, IControlBehaviour
    {
        private const int fixed_fps = 60;

        private World map;
        private Input input;
        

        public RendererBox()
        {
            InitializeComponent();
        }

        public void OnClosed(object sender)
        {
            CIntegrations.Release();
        }

        public void OnLoad(object sender)
        {
            Resourcepack.Loadresources();

            Service.Tiling.LoadTilesFromMap("tilemap",
               new TilemapCell("grass", 640, 0, 32, 32),
               new TilemapCell("tree", 384, 448, 64, 64));

            map = new World();
            input = new Input();

            Graphic.Create(Width, Height, Handle);
        }

        public void OnPaint(object sender)
        {
            if (map.GetCameraObject() != null)
                map.GetCameraObject().camera.Renderer();

            Graphic.graphics.Draw();

            Text = $"{1000 / Time.deltaTime} FPS";
        }
    }
}
