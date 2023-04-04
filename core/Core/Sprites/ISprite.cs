using System.Drawing;

namespace Core.Display.Sprites;

public interface ISprite
{
    Color? GetColor(int frameNo, int x, int y);
    int Width { get; }
    int Height { get; }
    void Draw(IDisplay display, int x, int y, int frameNo = 0);
}