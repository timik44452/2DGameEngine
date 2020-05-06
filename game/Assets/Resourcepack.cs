using System.Linq;
using System.IO;
using System.Collections.Generic;


public static class Resourcepack
{
    private static List<Asset> assets = new List<Asset>();

    public static T GetResource<T>(string name) where T : Asset
    {
        return (T)assets.Find(x => x.GetType() == typeof(T) && x.name == name);
    }

    public static IEnumerator<T> GetResources<T>(params string[] names) where T : Asset
    {
        foreach (string name in names)
        {
            yield return (T)assets.Find(x => x.name == name);
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

            AddAsset(info.Name.Replace(".png", ""), texture, info.FullName);
            AddAsset(info.Name.Replace(".png", ""), sprite);
        }

        foreach (var info in new DirectoryInfo(path).GetFiles("*.hlsl"))
        {
            Shader shader = new Shader(info.FullName);

            AddAsset(info.Name.Replace(".hlsl", ""), shader);
        }
    }

    public static void AddAsset(string name, object asset, string path = "local")
    {
        Asset value = asset as Asset;

        if(value != null)
        {
            value.UID = GetNextUID();
            value.path = path;
            value.name = name;

            assets.Add(value);
        }
        else 
        {
            throw new System.NotImplementedException($"asset type hasn't valid");
        }
    }

    private static int GetNextUID()
    {
        if(assets.Count == 0)
        {
            return 0;
        }

        return assets.Max(x => x.UID) + 1;
    }
}