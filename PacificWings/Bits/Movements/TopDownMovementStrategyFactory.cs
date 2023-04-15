using System.Numerics;
using Core.Display.Sprites;
using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace PacificWings.Bits.Movements;

public class TopDownStrategyStrategy : IMovementStrategyFactory
{
    private readonly int total;
    private readonly int spacing;
    private readonly ISprite sprite;

    public TopDownStrategyStrategy(int total, int spacing, ISprite sprite)
    {
        this.total = total;
        this.spacing = spacing;
        this.sprite = sprite;
    }
    
    public IMovementStrategy Create(int no)
    {
        var targets = new List<Vector2>
        {
            new(0, 0),
            new(0, 64)
        };
        const int delay = 0;
        var left = total * (sprite.Width + spacing) / 2;
        var offset = new Vector2(left + no * (sprite.Width + spacing), 0);
        return new BezierMovementStrategyStrategy(targets, delay, offset);
    }
}