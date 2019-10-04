
namespace WindowsFormsApp5.Service
{
    public static class Tiling
    {
        public static void LoadTilesFromMap(string name, params TilemapCell[] cells)
        {
            Texture tilemap = Resourcepack.GetResource<Texture>(name);

            if (tilemap != null)
            {
                for (int k = 0; k < cells.Length; k++)
                {
                    TilemapCell cell = cells[k];
                    Texture tile = new Texture(cell.Width, cell.Height);
                    

                    ImageProcessor.Copy(new Texture(cell.Width, cell.Height), tile);

                    Resourcepack.AddAsset(cell.name, tile);
                    Resourcepack.AddAsset(cell.name, new Sprite(tile));
                }
            }
        }
    }
}
