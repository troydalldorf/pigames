using System.Drawing;
using Core.Display.LedMatrix;

namespace Core.Display.Fonts;

public class LedFont : IDisposable
{
    private RgbLedFont font;
    public LedFont(LedFontType fontType) : this(LedFont.GetBdfFilePath(fontType))
    {
    }

    private LedFont(string bdfFilePath)
    {
        font = new RgbLedFont(bdfFilePath);
    }

    public void DrawText(LedDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false)
    {
        font.DrawText(display.GetCanvas()._canvas, x, y, color, text, spacing, vertical);
    }

    private static string GetBdfFilePath(LedFontType fontType)
    {
        var fontPath = fontType switch
        {
            _ => $"./Fonts/{fontType.ToString().Replace("Font", "")}.bdf"
        };
        if (!File.Exists(fontPath))
            throw new FileNotFoundException($"File, '{fontPath}' for font type '{fontType}' does not exist.");
        return fontPath;
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