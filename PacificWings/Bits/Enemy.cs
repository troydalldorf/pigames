using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Effects;
using PacificWings.Bits.Movements;

namespace PacificWings.Bits;

public class Enemy
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; } = 8;
    public int Height { get; } = 8;
    public EnemyState State { get; private set; }

    public int Speed { get; set; }
    private readonly IEnemyMovement movementStrategy;
    private readonly SpriteAnimation sprite;
    private SpriteBomb explosion;

    public Enemy(int x, int y, int speed, IEnemyMovement movementStrategy, SpriteAnimation sprite)
    {
        X = x;
        Y = y;
        this.Speed = speed;
        this.movementStrategy = movementStrategy;
        this.sprite = sprite;
        State = EnemyState.Alive;
    }

    private void Move()
    {
        if (State != EnemyState.Alive) return;
        if (!this.movementStrategy.Move(this))
            State = EnemyState.Destroyed;
    }

    public void Update(List<Bullet> bullets)
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
            explosion = new SpriteBomb(this.X, this.Y, this.sprite);
            State = EnemyState.Exploding;
            bullets.Remove(bullet);
            break;
        }
    }

    public void Draw(IDisplay display)
    {
        switch (State)
        {
            case EnemyState.Alive:
                this.sprite.Draw(display, X, Y);
                break;
            case EnemyState.Exploding:
                this.explosion.Draw(display);
                break;
        }
    }

    private bool IsCollidingWithBullet(Bullet bullet)
    {
        return bullet.X >= X && bullet.X <= X + Width && bullet.Y >= Y && bullet.Y <= Y + Height;
    }

    public enum EnemyState
    {
        Alive,
        Exploding,
        Destroyed,
    }
}