using Core.Fonts;

namespace Core.PiConsole.Fonts;

public class FontFactory : IFontFactory
{
    public IFont GetFont(LedFontType fontType)
    {
        return new LedFont(fontType);
    }
}