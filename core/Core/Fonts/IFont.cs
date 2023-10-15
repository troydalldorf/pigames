using System.Drawing;
using Core.Display;

namespace Core.Fonts;

public interface IFont
{
    void DrawText(IDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false);
}