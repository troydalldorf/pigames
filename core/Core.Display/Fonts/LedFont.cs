using System.Drawing;
using Core.Display.LedMatrix;

namespace Core.Display.Fonts;

public class LedFont : IDisposable
{
    private RgbLedFont font;
    public LedFont(LedFontType fontType) : this(LedFont.GetBdfFilePath(fontType))
    {
    }

    public LedFont(string bdfFilePath)
    {
        font = new RgbLedFont(bdfFilePath);
    }

    public void DrawText(LedDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false)
    {
        font.DrawText(display.GetCanvas()._canvas, x, y, color, text, spacing, vertical);
    }

    private static string GetBdfFilePath(LedFontType fontType)
    {
        return fontType switch
        {
            _ => $"{fontType.ToString().Replace("Font", "")}.bdf"
        };
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            font.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LedFont()
    {
        Dispose(false);
    }
}