using System.Numerics;

namespace PacificWings.Bits.Movements;

public class BezierMovementStrategy : IMovementStrategy
{
    private readonly List<Vector2> targets;
    private int currentTargetIndex;
    private readonly int delay;
    private readonly Vector2 offset;
    private int moveCount;
    private Vector2 currentPosition;

    public BezierMovementStrategy(List<Vector2> targets, int delay, Vector2 offset)
    {
        this.targets = targets;
        currentTargetIndex = 0;
        this.delay = delay;
        this.offset = offset;
        moveCount = 0;
        StartPosition = targets[0] + offset;
        currentPosition = StartPosition;
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

        var target = targets[currentTargetIndex + 1] + offset;
        var direction = Vector2.Normalize(target - currentPosition);
        var newPosition = currentPosition + direction * enemy.Speed;

        if (Vector2.Distance(newPosition, target) < enemy.Speed)
        {
            currentPosition = target;
            currentTargetIndex++;
        }
        else
        {
            currentPosition = newPosition;
        }

        enemy.X = (int)currentPosition.X;
        enemy.Y = (int)currentPosition.Y;

        return true;
    }
}