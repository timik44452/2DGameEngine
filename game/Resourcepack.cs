using System.IO;
using System.Collections.Generic;


public static class Resourcepack
{
    private static List<Asset> assets = new List<Asset>();

    public static T GetResource<T>(string name) where T : Asset
    {
        return (T)assets.Find(x => x.name == name);
    }

    public static IEnumerator<T> GetResources<T>(params string[] names) where T : Asset
    {
        foreach (string name in names)
        {
            yield return (T)assets.Find(x => x.name == name);
        }
    }
    
    public static void LoadTilemap(params TilemapCell[] cells)
    {
        Texture tilemap = GetResource<Texture>("texture");

        if(tilemap != null)
        {
            for (int k = 0; k < cells.Length; k++)
            {
                TilemapCell cell = cells[k];
                Texture texture = new Texture(cell.Width, cell.Height);

                for (int y = 0; y < cell.Height; y++)
                {
                    for (int x = 0; x < cell.Width; x++)
                    {
                        texture.SetPixel(x, y, tilemap.GetPixel(cell.X + x, cell.Y + y));
                    }
                }

                AddAsset(cell.name, texture);
                AddAsset(cell.name, new Sprite(texture));
            }
        }
    }

    public static void Loadresources()
    {
        string path = @"Resources";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach (var info in new DirectoryInfo(path).GetFiles("*.png"))
        {
            Texture texture = new Texture(info.FullName);
            Sprite sprite = new Sprite(texture);

            AddAsset(info.Name.Replace(".png", ""), texture);
            AddAsset(info.Name.Replace(".png", ""), sprite);
        }

        foreach (var info in new DirectoryInfo(path).GetFiles("*.hlsl"))
        {
            Shader shader = new Shader(info.FullName);

            AddAsset(info.Name.Replace(".hlsl", ""), shader);
        }
    }

    public static void AddAsset(string name, object asset)
    {
        Asset value = asset as Asset;

        if(value != null)
        {
            value.name = name;

            assets.Add(value);
        }
        else 
        {
            throw new System.NotImplementedException($"asset type hasn't valid");
        }
    }
}