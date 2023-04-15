using System.Numerics;
using Core.Display.Sprites;
using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace PacificWings.Bits.Movements;

public class TopDown : IMovementStrategyFactory
{
    private readonly int total;
    private readonly int spacing;
    private readonly ISprite sprite;

    public TopDown(int total, int spacing, ISprite sprite)
    {
        this.total = total;
        this.spacing = spacing;
        this.sprite = sprite;
    }
    
    public IMovementStrategy Create(int no)
    {
        var targets = new List<Point>
        {
            new(0, -sprite.Height),
            new(0, 64)
        };
        const int delay = 0;
        var left = (64 - total * (sprite.Width + spacing)) / 2;
        var offset = new Point(left + no * (sprite.Width + spacing), 0);
        return new BezierMovementStrategy(targets.ToArray(), delay, offset);
    }
}