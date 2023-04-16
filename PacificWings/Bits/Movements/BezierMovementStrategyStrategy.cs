using PacificWings.Bits;
using PacificWings.Bits.Movements;

public class BezierMovementStrategy : IMovementStrategy
{
    private readonly List<Point> targets;
    private readonly int delay;
    private readonly Point offset;
    private int moveCount;
    private Point currentPosition;
    private float t;

    public BezierMovementStrategy(List<Point> targets, int delay, Point offset)
    {
        this.targets = targets;
        this.delay = delay;
        this.offset = offset;
        moveCount = 0;
        StartPosition = new Point(targets[0].X + offset.X, targets[0].Y + offset.Y);
        currentPosition = StartPosition;
        t = 0;
    }

    public Point StartPosition { get; }

    public bool Move(Enemy enemy)
    {
        if (moveCount < delay)
        {
            moveCount++;
            return true;
        }

        if (t >= 1)
        {
            return false;
        }

        currentPosition = BezierPoint(t, targets);
        enemy.X = (int)(currentPosition.X + offset.X);
        enemy.Y = (int)(currentPosition.Y + offset.Y);

        t += enemy.Speed / 1000f; // Adjust the divisor to control the speed of the movement along the curve
        return true;
    }

    private static Point BezierPoint(float t, List<Point> controlPoints)
    {
        if (controlPoints.Count == 1)
        {
            return controlPoints[0];
        }

        List<Point> newControlPoints = new List<Point>();

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            float newX = controlPoints[i].X + (controlPoints[i + 1].X - controlPoints[i].X) * t;
            float newY = controlPoints[i].Y + (controlPoints[i + 1].Y - controlPoints[i].Y) * t;
            newControlPoints.Add(new Point(newX, newY));
        }

        return BezierPoint(t, newControlPoints);
    }
}