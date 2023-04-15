using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace PacificWings.Bits;

public class Enemy
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; } = 8;
    public int Height { get; } = 8;
    public bool IsDestroyed { get; private set; }

    private int speed;
    private readonly SpriteAnimation sprite;

    public Enemy(int x, int y, int speed, SpriteAnimation sprite)
    {
        X = x;
        Y = y;
        this.speed = speed;
        this.sprite = sprite;
        IsDestroyed = false;
    }

    public void Move()
    {
        if (IsDestroyed) return;

        Y += speed;

        if (Y > 64)
        {
            IsDestroyed = true;
        }
    }

    public void Update(List<Bullet> bullets)
    {
        Move();

        // Check for bullet collisions
        foreach (var bullet in bullets.ToArray())
        {
            if (!IsCollidingWithBullet(bullet)) continue;
            IsDestroyed = true;
            bullets.Remove(bullet);
            break;
        }
    }

    public void Draw(IDisplay display)
    {
        if (!IsDestroyed)
        {
            this.sprite.Draw(display, X, Y);
        }
    }

    private bool IsCollidingWithBullet(Bullet bullet)
    {
        return bullet.X >= X && bullet.X <= X + Width && bullet.Y >= Y && bullet.Y <= Y + Height;
    }
}