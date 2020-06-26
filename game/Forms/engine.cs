using System;
using System.Windows.Forms;


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

    private void OnClose(object sender, FormClosingEventArgs eventArgs)
    {
        try
        {
            control?.OnClosed(sender);
        }
        catch(Exception e)
        {
            MessageBox.Show("Closing error", e.Message, MessageBoxButtons.OK);
        }
    }

    private void OnLoad(object sender, EventArgs eventArgs)
    {
        try
        {
            control?.OnLoad(sender);
        }
        catch (Exception e)
        {
            MessageBox.Show("Loading error", e.Message, MessageBoxButtons.OK);
        }
    }
}