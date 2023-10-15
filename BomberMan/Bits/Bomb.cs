using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Sprites;

namespace BomberMan.Bits;

public class Bomb : IGameElement
{
    public int X { get; }
    public int Y { get; }
    public bool IsExploded => timer.ElapsedMilliseconds >= timeout;
    private readonly Timer timer;
    private readonly int timeout;
    private readonly Sprite sprite;

    public Bomb(int x, int y, int timeout, Sprite sprite)
    {
        X = x;
        Y = y;
        this.timeout = timeout;
        this.sprite = sprite;
        timer = new Timer();
        timer.Start();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        sprite.Draw(display, X*8, Y*8);
    }

    public bool CollidesWith(int player1X, int player1Y)
    {
        var bombRect = new Rectangle(X * 8, Y * 8, 8, 8);
        var playerRect = new Rectangle(player1X * 8, player1Y * 8, 8, 8);
        return bombRect.IntersectsWith(playerRect);
    }
}