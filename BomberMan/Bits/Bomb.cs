using System.Drawing;
using Core;
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
}