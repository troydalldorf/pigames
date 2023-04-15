using System;
using System.Collections.Generic;
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
        Console.Write("move");
        if (moveCount < delay)
        {
            moveCount++;
            return true;
        }

        if (currentTargetIndex == targets.Count - 1)
        {
            return false;
        }

        Point target = new Point(targets[currentTargetIndex + 1].X + offset.X, targets[currentTargetIndex + 1].Y + offset.Y);
        double distanceToTarget = Distance(currentPosition, target);
        double moveFraction = enemy.Speed / distanceToTarget;

        if (moveFraction >= 1)
        {
            currentPosition = target;
            currentTargetIndex++;
        }
        else
        {
            int newX = currentPosition.X + (int)((target.X - currentPosition.X) * moveFraction);
            int newY = currentPosition.Y + (int)((target.Y - currentPosition.Y) * moveFraction);
            currentPosition = new Point(newX, newY);
        }

        enemy.X = currentPosition.X;
        enemy.Y = currentPosition.Y;

        return true;
    }

    private double Distance(Point p1, Point p2)
    {
        int dx = p2.X - p1.X;
        int dy = p2.Y - p1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}