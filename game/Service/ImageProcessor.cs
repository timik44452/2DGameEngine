
namespace WindowsFormsApp5.Service
{
    public static class ImageProcessor
    {
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
