using System.Drawing;
using Core.Display;

namespace Core.Fonts;

public static class FontExtensions
{
    public static void DrawTextWithBorder(this IFont @this, IDisplay display, int x, int y, Color color, Color borderColor, string text, int spacing = 0, int borderOffset = 1, bool vertical = false)
    {
        @this.DrawText(display, x - borderOffset, y - borderOffset, borderColor, text, spacing, vertical);
        @this.DrawText(display, x + borderOffset, y + borderOffset, borderColor, text, spacing, vertical);
        @this.DrawText(display, x, y, color, text, spacing, vertical);
    }
}