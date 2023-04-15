using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace PacificWings.Bits;

public class Bullet
{
    private readonly SpriteAnimation sprite;
    public int X { get; }
    public int Y { get; private set; }
    private int Speed { get; }
    public int Width => sprite.Width;
    public int Height => sprite.Height;

    public Bullet(int x, int y, int speed, SpriteAnimation sprite)
    {
        this.sprite = sprite;
        this.X = x;
        this.Y = y;
        this.Speed = speed;
    }

    public void Update()
    {
        Y -= Speed;
    }

    public void Draw(IDisplay display)
    {
        this.sprite.Draw(display, this.X, this.Y, 0);
    }
}