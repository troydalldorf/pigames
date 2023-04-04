namespace Core.Fonts;

public interface IFontFactory
{
    IFont GetFont(LedFontType fontType);
}