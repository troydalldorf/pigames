using System.Drawing;

namespace Core.Display.Sprites;

public interface ISprite
{
    Color? GetColor(int frameNo, int x, int y);
    int Width { get; }
    int Height { get; }
    void Draw(LedDisplay display, int x, int y, int frameNo = 0);
}