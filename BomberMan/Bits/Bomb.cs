using System.Drawing;
using Core;

namespace BomberMan.Bits;

public class Bomb
{
    public int X { get; }
    public int Y { get; }
    public bool IsExploded => _timer.ElapsedMilliseconds >= _timeout;
    private Timer _timer;
    private int _timeout;

    public Bomb(int x, int y, int timeout)
    {
        X = x;
        Y = y;
        _timeout = timeout;
        _timer = new Timer();
        _timer.Start();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.DrawCircle(X * 8 + 4, Y * 8 + 4, 4, Color.Black);
    }
}