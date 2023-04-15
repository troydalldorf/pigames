using System.Numerics;
using Core.Display.Sprites;
using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace PacificWings.Bits.Movements;

public class TopRightToBottomLeft : IMovementStrategyFactory
{
    private readonly int total;
    private readonly int spacing;
    private readonly int speed;
    private readonly ISprite sprite;

    public TopRightToBottomLeft(int total, int spacing, int speed, ISprite sprite)
    {
        this.total = total;
        this.spacing = spacing;
        this.speed = speed;
        this.sprite = sprite;
    }
    
    public IMovementStrategy Create(int no)
    {
        var targets = new List<Point>
        {
            new(48, -sprite.Height),
            new(32, 32),
            new(-sprite.Width, 64)
        };
        var delay = no * (sprite.Width + spacing) / this.speed;
        var offset = new Point(0, 0);
        return new BezierMovementStrategy(targets, delay, offset);
    }
}