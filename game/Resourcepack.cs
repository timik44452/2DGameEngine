using System.IO;
using System.Collections.Generic;


public static class Resourcepack
{
    private static Dictionary<string, Texture> texture_base = new Dictionary<string, Texture>();
    private static Dictionary<string, Sprite> sprite_base = new Dictionary<string, Sprite>();


    public static Sprite[] GetSprites(params string[] names)
    {
        Sprite[] sprites = new Sprite[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            sprites[i] = sprite_base.ContainsKey(names[i]) ? sprite_base[names[i]] : null;
        }

        return sprites;
    }

    public static Sprite GetSprite(string name)
    {
        if (sprite_base.ContainsKey(name))
            return sprite_base[name];
        else
            return null;
    }

    public static Texture GetTexture(string name)
    {
        if (texture_base.ContainsKey(name))
            return texture_base[name];
        else
            return null;
    }

    public static void LoadTilemap(TilemapCell[] cells)
    {
        var tilemap = GetTexture("tilemap");

        if(tilemap != null)
        {
            // Tilemap celling here....
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

            texture_base.Add(info.Name.Replace(".png", ""), texture);
            sprite_base.Add(info.Name.Replace(".png", ""), sprite);
        }
    }
}