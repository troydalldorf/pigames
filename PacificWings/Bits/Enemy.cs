using Core;
using Core.Display.Sprites;
using Core.Effects;
using Core.Sprites;
using PacificWings.Bits.Movements;
using Point = System.Drawing.Point;

namespace PacificWings.Bits;

public class Enemy
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; } = 8;
    public int Height { get; } = 8;
    public EnemyState State { get; private set; }

    public int Speed { get; set; }
    private readonly IMovementStrategy movementStrategyStrategy;
    private readonly SpriteAnimation sprite;
    private SpriteBomb explosion;
    private readonly LinkedList<Point> lastPositions;

    public Enemy(int x, int y, int speed, IMovementStrategy movementStrategyStrategy, SpriteAnimation sprite)
    {
        X = x;
        Y = y;
        this.Speed = speed;
        this.movementStrategyStrategy = movementStrategyStrategy;
        this.sprite = sprite;
        State = EnemyState.Alive;
        lastPositions = new LinkedList<Point>();
        lastPositions.AddLast(new Point(X, Y));
        lastPositions.AddLast(new Point(X, Y));
        lastPositions.AddLast(new Point(X, Y));
    }

    private void Move()
    {
        if (State != EnemyState.Alive) return;
        if (!this.movementStrategyStrategy.Move(this))
        {
            State = EnemyState.Destroyed;
        }
        else
        {
            lastPositions.AddLast(new Point(X, Y));
            if (lastPositions.Count > 3)
            {
                lastPositions.RemoveFirst();
            }
        }
    }

    public void Update(List<Bullet> bullets, Player player)
    {
        Move();

        if (State == EnemyState.Exploding)
        {
            explosion.Update();
            if (explosion.IsExtinguished())
                State = EnemyState.Destroyed;
        }
        
        foreach (var bullet in bullets.ToArray())
        {
            if (!IsCollidingWithBullet(bullet) || State != EnemyState.Alive) continue;
            player.AddScore(1);
            explosion = new SpriteBomb(this.X, this.Y, this.sprite);
            State = EnemyState.Exploding;
            bullets.Remove(bullet);
            break;
        }
    }

    public void Draw(IDisplay display)
    {
        var angle = CalculateAngle();

        switch (State)
        {
            case EnemyState.Alive:
                this.sprite.DrawRotated(display, X, Y, angle);
                break;
            case EnemyState.Exploding:
                this.explosion.Draw(display);
                break;
        }
    }
    
    private double CalculateAngle()
    {
        var p1 = lastPositions.First.Value;
        var p2 = lastPositions.First.Next.Value;
        var p3 = lastPositions.Last.Value;

        double dx1 = p2.X - p1.X;
        double dy1 = p2.Y - p1.Y;
        double dx2 = p3.X - p2.X;
        double dy2 = p3.Y - p2.Y;

        var avgDx = (dx1 + dx2) / 2;
        var avgDy = (dy1 + dy2) / 2;

        var angle = Math.Atan2(avgDy, avgDx);
        return angle * 180 / Math.PI + 270;
    }

    private bool IsCollidingWithBullet(Bullet bullet)
    {
        return bullet.X + bullet.Width >= X && bullet.X <= X + Width && bullet.Y + bullet.Height >= Y && bullet.Y <= Y + Height;
    }

    public enum EnemyState
    {
        Alive,
        Exploding,
        Destroyed,
    }
}