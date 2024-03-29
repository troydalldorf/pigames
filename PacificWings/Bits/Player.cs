using Core;
using Core.Display;
using Core.Display.Sprites;

namespace PacificWings.Bits;

public class Player : IGameElement
{
    private readonly SpriteAnimation playerSprite;
    private readonly SpriteAnimation bulletSprite;
    private readonly TimeSpan shotDelay = TimeSpan.FromMilliseconds(200);
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; } = 8;
    public int Height { get; } = 8;
    public int Score { get; private set; }
    public bool IsDestroyed { get; private set; }
    public List<Bullet> Bullets { get; }

    private const int Speed = 2;
    private const int BulletSpeed = 4;
    private DateTime lastShot = DateTime.MinValue;

    public Player(int x, int y, SpriteAnimation playerSprite, SpriteAnimation bulletSprite)
    {
        this.playerSprite = playerSprite;
        this.bulletSprite = bulletSprite;
        this.X = x;
        this.Y = y;
        this.IsDestroyed = false;
        this.Bullets = new List<Bullet>();
    }

    public void Move(JoystickDirection direction)
    {
        if (IsDestroyed) return;

        if ((direction & JoystickDirection.Up) == JoystickDirection.Up && Y > 0)
        {
            Y -= Speed;
        }
        if ((direction & JoystickDirection.Down) == JoystickDirection.Down && Y < 56)
        {
            Y += Speed;
        }
        if ((direction & JoystickDirection.Left) == JoystickDirection.Left && X > 0)
        {
            X -= Speed;
        }
        if ((direction & JoystickDirection.Right) == JoystickDirection.Right && X < 56)
        {
            X += Speed;
        }
    }

    public void Shoot(Buttons buttons)
    {
        if (IsDestroyed) return;

        if (DateTime.Now.Subtract(lastShot) < shotDelay)
            return;

        if (buttons.IsGreenPushed())
        {
            lastShot = DateTime.Now;
            Bullets.Add(new Bullet(X, Y, BulletSpeed, this.bulletSprite));
        }
    }

    public void Update()
    {
        Bullets.ForEach(bullet => bullet.Update());
        Bullets.RemoveAll(bullet => bullet.Y < 0);
    }

    public void Draw(IDisplay display)
    {
        if (!IsDestroyed)
        {
            this.playerSprite.Draw(display, this.X, this.Y, 0);
        }

        foreach (var bullet in Bullets)
        {
            bullet.Draw(display);
        }
    }

    public void Destroy()
    {
        IsDestroyed = true;
    }

    public void AddScore(int points)
    {
        Score += points;
    }
}
