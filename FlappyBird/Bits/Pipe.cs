using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace FlappyBird.Bits;

public class Pipe : IGameElement
{
    public int X { get; private set; }
    public Rectangle Rectangle => new(X, y, width, height);
    private readonly int y;
    private readonly int width;
    private readonly int height;
    private readonly SpriteAnimation sprite;
    public bool IsTop { get; }

    public Pipe(int x, int y, int width, int height, bool isTop, SpriteAnimation sprite)
    {
        this.X = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.sprite = sprite;
        this.IsTop = isTop;
    }

    public void Update()
    {
        X -= 1;
    }
    
    public void Draw(IDisplay display)
    {
        // if (IsTop)
        // {
        //     for (var row=0; row<height/sprite.Height; row++)
        //     {
        //         sprite.Draw(display, X, y + row * sprite.Height, r);
        //     }
        //     sprite.Draw(display, X, y);
        // }
        // else
        // {
        //     sprite.Draw(display, X, y, SpriteFlip.Vertical);
        // }
    }
}