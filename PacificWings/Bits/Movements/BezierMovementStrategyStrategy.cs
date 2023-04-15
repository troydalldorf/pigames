using PacificWings.Bits;
using PacificWings.Bits.Movements;
using System;

public class BezierMovementStrategy : IMovementStrategy
{
    private readonly Point[] controlPoints;
    private readonly int delay;
    private readonly Point offset;
    private int moveCount;
    private Point currentPosition;
    private float t;

    public BezierMovementStrategy(Point[] controlPoints, int delay, Point offset)
    {
        if (controlPoints.Length != 3)
        {
            throw new ArgumentException("Quadratic Bezier curve requires exactly 3 control points.");
        }

        this.controlPoints = controlPoints;
        this.delay = delay;
        this.offset = offset;
        moveCount = 0;
        StartPosition = new Point(controlPoints[0].X + offset.X, controlPoints[0].Y + offset.Y);
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

        currentPosition = CalculateBezierPoint(t, controlPoints[0], controlPoints[1], controlPoints[2]);
        enemy.X = (int)(currentPosition.X + offset.X);
        enemy.Y = (int)(currentPosition.Y + offset.Y);
        t += enemy.Speed;

        return true;
    }

    private Point CalculateBezierPoint(float t, Point p0, Point p1, Point p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        float x = uu * p0.X + 2 * u * t * p1.X + tt * p2.X;
        float y = uu * p0.Y + 2 * u * t * p1.Y + tt * p2.Y;

        return new Point(x, y);
    }
}