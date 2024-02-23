using System.Drawing;

namespace RomajiConverter.App.Extensions;

public static class ColorExtension
{
    public static string ToHexString(this Color color)
    {
        return "#" + color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
    }

    public static Color ToDrawingColor(this string hexString)
    {
        return (Color)new ColorConverter().ConvertFromString(hexString);
    }

    public static Avalonia.Media.Color ToAvaloniaColor(this string hexString)
    {
        return Avalonia.Media.Color.Parse(hexString);
    }
}