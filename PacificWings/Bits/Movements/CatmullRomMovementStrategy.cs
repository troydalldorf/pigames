using PacificWings.Bits;
using PacificWings.Bits.Movements;

public class CatmullRomMovementStrategy : IMovementStrategy
{
    private readonly List<Point> targets;
    private readonly int delay;
    private readonly Point offset;
    private int moveCount;
    private Point currentPosition;
    private float t;

    public CatmullRomMovementStrategy(List<Point> targets, int delay, Point offset)
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

        currentPosition = CatmullRomPoint(t, targets);
        enemy.X = (int)(currentPosition.X + offset.X);
        enemy.Y = (int)(currentPosition.Y + offset.Y);

        t += enemy.Speed / 100f; // Adjust the divisor to control the speed of the movement along the curve
        return true;
    }

    private static Point CatmullRomPoint(float t, List<Point> controlPoints)
    {
        int segmentCount = controlPoints.Count - 3;
        float tPerSegment = 1f / segmentCount;
        int segmentIndex = (int)(t / tPerSegment);
        float segmentT = (t - segmentIndex * tPerSegment) / tPerSegment;

        Point p0 = controlPoints[Math.Max(0, segmentIndex - 1)];
        Point p1 = controlPoints[segmentIndex];
        Point p2 = controlPoints[Math.Min(segmentIndex + 1, controlPoints.Count - 1)];
        Point p3 = controlPoints[Math.Min(segmentIndex + 2, controlPoints.Count - 1)];

        float tt = segmentT * segmentT;
        float ttt = tt * segmentT;

        float q1 = -ttt + 2.0f * tt - segmentT;
        float q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
        float q3 = -3.0f * ttt + 4.0f * tt + segmentT;
        float q4 = ttt - tt;

        float tx = 0.5f * (p0.X * q1 + p1.X * q2 + p2.X * q3 + p3.X * q4);
        float ty = 0.5f * (p0.Y * q1 + p1.Y * q2 + p2.Y * q3 + p3.Y * q4);

        return new Point(tx, ty);
    }
}
