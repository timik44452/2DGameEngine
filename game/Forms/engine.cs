using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class engine : Form
    {
        private const bool IsFullWindow = false;
        
        private IControlBehaviour control;

        public engine()
        {
            InitializeComponent();

            if (IsFullWindow)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            control = rendererBox;

            Timer timer = new Timer();
            timer.Tick += (s, e) => OnPaintScene(s);
            timer.Interval = 1;
            timer.Start();
        }

        private void OnPaintScene(object sender)
        {
            control?.OnPaint(sender);
            Text = $"FPS:{1000F / GameMath.Max(control.deltaTime, 1)}";
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            control?.OnClosed(sender);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            control?.OnLoad(sender);
        }
    }
}