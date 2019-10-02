using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBox1_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                Bitmap image = (Bitmap)Image.FromFile(file);

                string[] lines = new string[image.Height];

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        var pixel = image.GetPixel(x, y);

                        //int value = Compressor.Compress(pixel.R, pixel.G, pixel.B);

                        //lines[y] += $"{value}, ";
                    }

                }

                textBox1.Lines = lines;
            }
        }

        private void TextBox1_DragLeave(object sender, EventArgs e)
        {
            
        }
    }
}
