using System.Drawing;

namespace Breakout.Bits;

public enum PowerUpType
{
    PaddleSizeIncrease,
    MultiBall,
    StickyPaddle
}

public class PowerUp
{
    public PowerUpType Type { get; set; }
    public Rectangle Bounds { get; set; }
    public Color Color { get; set; }
    public bool IsActive { get; set; }

    public PowerUp(PowerUpType type, Rectangle bounds, Color color)
    {
        Type = type;
        Bounds = bounds;
        Color = color;
        IsActive = true;
    }
}