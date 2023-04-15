namespace PacificWings.Bits;

public interface IEnemyMovement
{
    (int, int) Start(int enemyNo, int enemyWidth, int enemyHeight, int enemySpacing);
    bool Move(Enemy enemy);
}

public class RightToLeftMovement : IEnemyMovement
{
    public (int, int) Start(int enemyNo, int enemyWidth, int enemyHeight, int enemySpacing)
    {
        return (64 + enemyNo * (enemyWidth + enemySpacing), 8);
    }

    public bool Move(Enemy enemy)
    {
        enemy.X -= enemy.Speed;
        return enemy.X + enemy.Width > 0;
    }
}

public class LeftToRightMovement : IEnemyMovement
{
    public (int, int) Start(int enemyNo, int enemyWidth, int enemyHeight, int enemySpacing)
    {
        return (-enemyNo * (enemyWidth + enemySpacing), 8);
    }

    public bool Move(Enemy enemy)
    {
        enemy.X += enemy.Speed;
        return enemy.X < 64;
    }
}

public class TopDownMovement : IEnemyMovement
{
    public (int, int) Start(int enemyNo, int enemyWidth, int enemyHeight, int enemySpacing)
    {
        return (enemyNo * (enemyWidth + enemySpacing), -enemyHeight);
    }

    public bool Move(Enemy enemy)
    {
        enemy.Y += enemy.Speed;
        return enemy.Y < 64;
    }
}

public class CircularMovement : IEnemyMovement
{
    private double angle = 0;
    private int stage = 0;

    public (int, int) Start(int enemyNo, int enemyWidth, int enemyHeight, int enemySpacing)
    {
        stage = 0;
        return (64 + enemyNo * (enemyWidth + enemySpacing), 8 - enemyNo * (enemyWidth + enemySpacing));
    }

    public bool Move(Enemy enemy)
    {
        if (stage == 0) // Move down to the lower left
        {
            enemy.Y += enemy.Speed;
            enemy.X -= enemy.Speed;

            if (enemy.Y >= 40)
            {
                stage = 1;
            }
        }
        else if (stage == 1) // Circle
        {
            angle += 0.05;
            enemy.X = 16 + (int)(16 * Math.Cos(angle));
            enemy.Y = 48 + (int)(16 * Math.Sin(angle));

            if (angle >= 2 * Math.PI)
            {
                stage = 2;
            }
        }
        else if (stage == 2) // Exit to the lower left
        {
            enemy.Y += enemy.Speed;
            enemy.X -= enemy.Speed;
        }

        return enemy.X < 64 && enemy.Y < 64;
    }
}
