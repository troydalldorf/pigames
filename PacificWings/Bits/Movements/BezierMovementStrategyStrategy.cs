using PacificWings.Bits;
using PacificWings.Bits.Movements;

public class BezierMovementStrategy : IMovementStrategy
{
    private readonly List<Point> targets;
    private int currentTargetIndex;
    private readonly int delay;
    private readonly Point offset;
    private int moveCount;
    private Point currentPosition;

    public BezierMovementStrategy(List<Point> targets, int delay, Point offset)
    {
        this.targets = targets;
        currentTargetIndex = 0;
        this.delay = delay;
        this.offset = offset;
        moveCount = 0;
        StartPosition = new Point(targets[0].X + offset.X, targets[0].Y + offset.Y);
        currentPosition = StartPosition;
    }

    public Point StartPosition { get; }

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

        var target = new Point(targets[currentTargetIndex + 1].X + offset.X, targets[currentTargetIndex + 1].Y + offset.Y);
        var distanceToTarget = Distance(currentPosition, target);
        var moveFraction = enemy.Speed / distanceToTarget;

        Console.WriteLine($"Speed: {enemy.Speed}, distanceToTarget: {distanceToTarget} Move Fraction: {moveFraction}, target: {target}, current: {currentPosition}");
        if (moveFraction >= 1)
        {
            currentPosition = target;
            currentTargetIndex++;
        }
        else
        {
            var newX = currentPosition.X + ((target.X - currentPosition.X) * moveFraction);
            var newY = currentPosition.Y + ((target.Y - currentPosition.Y) * moveFraction);
            currentPosition = new Point(newX, newY);
        }

        enemy.X = (int)currentPosition.X;
        enemy.Y = (int)currentPosition.Y;

        return true;
    }

    private static float Distance(Point p1, Point p2)
    {
        var dx = p2.X - p1.X;
        var dy = p2.Y - p1.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}