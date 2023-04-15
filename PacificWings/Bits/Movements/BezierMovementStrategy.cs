using System.Numerics;

namespace PacificWings.Bits.Movements;

public class BezierMovementStrategy : IEnemyMovement
{
    private readonly List<Vector2> targets;
    private int currentTargetIndex;
    private readonly int delay;
    private readonly Vector2 offset;
    private int moveCount;

    public BezierMovementStrategy(List<Vector2> targets, int delay, Vector2 offset)
    {
        this.targets = targets;
        currentTargetIndex = 0;
        this.delay = delay;
        this.offset = offset;
        moveCount = 0;
        StartPosition = targets[0] + offset;
    }

    public Vector2 StartPosition { get; }

    public bool Move(Enemy enemy)
    {
        if (moveCount < delay)
        {
            moveCount++;
            return true;
        }

        if (currentTargetIndex == targets.Count - 1)
        {
            return false;
        }

        float t = enemy.Speed;
        var newPosition = CalculateBezierPoint(t, targets[currentTargetIndex], targets[currentTargetIndex + 1]);

        newPosition += offset;

        enemy.X = (int)newPosition.X;
        enemy.Y = (int)newPosition.Y;

        if (Vector2.Distance(newPosition, targets[currentTargetIndex + 1] + offset) < 0.001f)
        {
            currentTargetIndex++;
        }

        return true;
    }

    private static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1)
    {
        return p0 * (1 - t) + p1 * t;
    }
}