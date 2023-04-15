namespace PacificWings.Bits;

public interface IEnemyMovement
{
    void Move(Enemy enemy);
}

public class RightToLeftMovement : IEnemyMovement
{
    public void Move(Enemy enemy)
    {
        enemy.X -= enemy.Speed;
    }
}

public class LeftToRightMovement : IEnemyMovement
{
    public void Move(Enemy enemy)
    {
        enemy.X += enemy.Speed;
    }
}

public class TopDownMovement : IEnemyMovement
{
    public void Move(Enemy enemy)
    {
        enemy.Y += enemy.Speed;
    }
}

public class CircularMovement : IEnemyMovement
{
    private double angle = 0;

    public void Move(Enemy enemy)
    {
        angle += 0.05;
        enemy.X += (int)(enemy.Speed * Math.Cos(angle));
        enemy.Y += (int)(enemy.Speed * Math.Sin(angle));
    }
}