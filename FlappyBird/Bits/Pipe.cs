using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace FlappyBird.Bits;

public class Pipe : IGameElement
{
    private int x;
    public Rectangle Rectangle => new(x, y, width, height);
    private readonly int y;
    private readonly int width;
    private readonly int height;
    private readonly SpriteAnimation sprite;
    public bool IsTop { get; }

    public Pipe(int x, int y, int width, int height, bool isTop, SpriteAnimation sprite)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.sprite = sprite;
        this.IsTop = isTop;
    }

    public void Update()
    {
        x -= 1;
    }
    
    public void Draw(IDisplay display)
    {
        var rows = height / sprite.Height;
        sprite.Draw(display, x, y, IsTop ? 1 : 2);
        for (var row = 1; row < rows-1; row++)
        {
            sprite.Draw(display, x, y + row * sprite.Height, 1);
        }
        sprite.Draw(display, x, y + (rows-1) * sprite.Height, IsTop ? 0 : 1);
    }
}