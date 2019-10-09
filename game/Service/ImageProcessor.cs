using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace Service
{
    public static class ImageProcessor
    {
        public static void Save(string path, int[] buffer, int Width, int Height)
        {
            using (Bitmap image = new Bitmap(Width, Height))
            {
                BitmapData data = image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

                image.UnlockBits(data);

                image.Save(path);
            }
        }

        public static void Copy(Texture source, Texture destination, int x_offset = 0, int y_offset = 0)
        {            
            for (int y = 0; y < destination.Height; y++)
            {
                for (int x = 0; x < destination.Width; x++)
                {
                    destination.SetPixel(x, y, source.GetPixel(x + x_offset, y + y_offset));
                }
            }
        }
    }
}