using System.Drawing;

namespace Breakout.Bits;

public record Brick(int X, int Y, Color Color)
{
    public const int BrickWidth = 6;
    public const int BrickHeight = 2;

    public bool IntersectsWith(Rectangle r)
    {
        var brickRight = X + BrickWidth;
        var brickBottom = Y + BrickHeight;
        return X < r.Right && brickRight > r.Left && Y < r.Bottom && brickBottom > r.Top;
    }
}