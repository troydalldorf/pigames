using System.Numerics;

namespace PacificWings.Bits.Movements;

public class TopDownStrategy : BezierMovementStrategy
{
    public TopDownStrategy(int offset, int enemyHeight) : base(new List<Vector2>
    {
        new(0, -enemyHeight),
        new(0, 64)
    }, 0, new Vector2(offset, 0))
    {
    }
}