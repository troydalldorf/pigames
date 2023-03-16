namespace Pacman;

public class PacMan
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; set; }
    public Direction Direction { get; set; }

    public PacMan(int x, int y, int speed)
    {
        X = x;
        Y = y;
        Speed = speed;
        Direction = Direction.None;
    }

    public void Move()
    {
        switch (Direction)
        {
            case Direction.Up:
                Y -= Speed;
                break;
            case Direction.Down:
                Y += Speed;
                break;
            case Direction.Left:
                X -= Speed;
                break;
            case Direction.Right:
                X += Speed;
                break;
            default:
                break;
        }
    }
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}