using Core.Fonts;

namespace Core.Display.Fonts;

public class FontFactory : IFontFactory
{
    public IFont GetFont(LedFontType fontType)
    {
        return new LedFont(fontType);
    }
}