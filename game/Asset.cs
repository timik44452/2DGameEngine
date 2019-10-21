
using System.Collections.Generic;

public class Asset
{
    public uint UID { get; set; }
    public string name { get; set; }
    public string path { get; set; } = "local";

    public override bool Equals(object obj)
    {
        return obj is Asset asset &&
               UID == asset.UID &&
               name == asset.name &&
               path == asset.path;
    }

    public override int GetHashCode()
    {
        var hashCode = 550320393;
        hashCode = hashCode * -1521134295 + UID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
        return hashCode;
    }
}