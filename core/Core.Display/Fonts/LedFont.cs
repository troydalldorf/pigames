using System.Drawing;
using Core.Display.LedMatrix;
using Core.Fonts;

namespace Core.Display.Fonts;

public class LedFont : IFont, IDisposable
{
    private RgbLedFont font;
    public LedFont(LedFontType fontType) : this(LedFont.GetBdfFilePath(fontType))
    {
    }

    private LedFont(string bdfFilePath)
    {
        font = new RgbLedFont(bdfFilePath);
    }

    public void DrawText(IDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false)
    {
        var rgbCanvas = ((RgbLedCanvas)((IDirectCanvasAccess)display).GetCanvas())._canvas;
        font.DrawText(rgbCanvas, x, y, color, text, spacing, vertical);
    }

    private static string GetBdfFilePath(LedFontType fontType)
    {
        var filename = fontType switch
        {
            LedFontType.FontClr6x12 => "clR6x12.bdf",
            LedFontType.FontHelveticaR12 => "helvR12.bdf",
            LedFontType.FontTexGyre27 => "texgyre-27.bdf",
            LedFontType.FontTomThumb => "tom-thumb.bdf",
            _ => $"{fontType.ToString().Replace("Font", "")}.bdf"
        };
        var basePath = AppContext.BaseDirectory;
       var  fontPath = Path.Combine(basePath, "Fonts", "Bdf", filename);
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