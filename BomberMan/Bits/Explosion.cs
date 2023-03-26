using System.Drawing;
using Core;

namespace BomberMan.Bits;

public class Explosion
{
    public int X { get; }
    public int Y { get; }
    public bool IsDone => _timer.ElapsedMilliseconds >= _duration;
    private Timer _timer;
    private int _duration;

    public Explosion(int x, int y, int duration)
    {
        X = x;
        Y = y;
        _duration = duration;
        _timer = new Timer();
        _timer.Start();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(X * 8, Y * 8, 8, 8, Color.Orange, Color.Orange);
    }

    public bool CollidesWith(int x, int y)
    {
        return X == x && Y == y;
    }
}