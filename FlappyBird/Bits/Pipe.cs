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
        var rows = height / sprite.Height + 1;
        for (var row = 0; row < rows; row++)
            sprite.Draw(display, x, y + row * sprite.Height, 1);
        if (IsTop)
            sprite.Draw(display, x, y+height-sprite.Height, 0);
        else
            sprite.Draw(display, x, y, 2);
    }
}