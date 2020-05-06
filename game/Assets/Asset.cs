
using System.Collections.Generic;

public class Asset
{
    public int UID { get; set; }
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
        return UID;
    }
}