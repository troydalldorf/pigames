using System.Drawing;

namespace Core.Display.Sprites;

public interface ISprite
{
    Color? GetColor(int x, int y);
    int Width { get; }
    int Height { get; }
}