public static class ColorHelper
{
    public static Color GetColor(byte R, byte G, byte B, byte A)
    {
        return new Color(R / 255F, G / 255F, B / 255F, A / 255F);
    }
}