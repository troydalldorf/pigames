using System.Drawing;

namespace Core.Effects;

public static class ColorExtensions
{
    public static Color Fade(this Color color, double fade)
    {
        return Color.FromArgb((int)(color.R * fade), (int)(color.G * fade), (int)(color.B * fade));
    }
}