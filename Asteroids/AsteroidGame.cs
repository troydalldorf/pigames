using System.Drawing;
using Asteroids.Bits;
using Core;
using Core.Display.Fonts;
using Core.Effects;
using Core.Fonts;

namespace Asteroids;

public class AsteroidsGame : IDuoPlayableGameElement
{
    private Ship p1Ship;
    private Ship p2Ship;
    private readonly List<Asteroid> asteroids;
    private readonly List<Bullet> bullets;
    private readonly IFont scoreFont;
    private readonly List<PixelBomb> pixelBombs = new();
    private const int DisplayWidth = 64;
    private const int DisplayHeight = 64;
    private const int MaxBullets = 250;
    private const int BulletLife = 64;
    private const float ShipRotationSpeed = 20f;
    private const float BulletSpeed = 4f;

    public AsteroidsGame(IFontFactory fontFactory)
    {
        SpawnP1Ship();
        SpawnP2Ship();
        asteroids = new List<Asteroid>();
        bullets = new List<Bullet>();
        scoreFont = new FontFactory().GetFont(LedFontType.FontTomThumb);
        State = GameOverState.None;
        InitializeAsteroids();
    }

    private void SpawnP1Ship(int lives = 3)
    {
        p1Ship = new Ship(DisplayWidth, DisplayHeight, Color.Red, lives, p1Ship?.Score ?? 0)
        {
            Location = new PointF(32, 27), Size = 2, Rotation = 0, Velocity = new PointF(0, 0), RotationSpeed = 0, Thrusting = false
        };
    }
    
    private void SpawnP2Ship(int lives = 3)
    {
        p2Ship = new Ship(DisplayWidth, DisplayHeight, Color.Blue, lives, p2Ship?.Score ?? 0)
        {
            Location = new PointF(32, 37), Size = 2, Rotation = 180, Velocity = new PointF(0, 0), RotationSpeed = 0, Thrusting = false
        };
    }

    private void InitializeAsteroids()
    {
        var random = new Random();
        for (var i = 0; i < 5; i++)
        {
            asteroids.Add(new Asteroid(DisplayWidth, DisplayHeight)
            {
                Location = new PointF(random.Next(0, 64), random.Next(0, 64)),
                Rotation = random.Next(0, 360),
                Velocity = new PointF((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1)),
                RotationSpeed = (float)(random.NextDouble() * 4 - 2),
                Size = 3
            });
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandlePlayerInput(player1Console, p1Ship);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandlePlayerInput(player2Console, p2Ship);
    }

    private void HandlePlayerInput(IPlayerConsole playerConsole, Ship ship)
    {
        var stick = playerConsole.ReadJoystick();
        var buttons = playerConsole.ReadButtons();
        ship.RotationSpeed = 0;
        ship.Thrusting = false;

        if (stick.IsLeft()) ship.RotationSpeed = -ShipRotationSpeed;
        if (stick.IsRight()) ship.RotationSpeed = ShipRotationSpeed;

        if (stick.IsUp() || buttons.IsBluePushed()) ship.Thrusting = true;

        // Fire bullet
        if (buttons.IsGreenPushed() && bullets.Count < MaxBullets)
        {
            var radians = ship.Rotation * (Math.PI / 180);
            bullets.Add(new Bullet(
                new PointF(ship.Location.X, ship.Location.Y),
                new PointF((float)Math.Cos(radians) * BulletSpeed, (float)Math.Sin(radians) * BulletSpeed),
                BulletLife, ship));
        }
    }

    public void Update()
    {
        p1Ship.Update();
        p2Ship.Update();

        // Update bullets
        for (var i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].Update();

            if (bullets[i].Life <= 0)
            {
                bullets.RemoveAt(i);
                continue;
            }

            // Check for bullet-asteroid collisions
            for (var j = asteroids.Count - 1; j >= 0; j--)
            {
                var asteroid = asteroids[j];
                if (!bullets[i].IsCollidingWith(asteroid)) continue;
                pixelBombs.Add(new PixelBomb((int)asteroid.Location.X, (int)asteroid.Location.Y, 5*asteroid.Size, asteroid.Color, 3));
                if (asteroids[j].Size > 1)
                {
                    asteroids.AddRange(asteroids[j].SpawnSmallerAsteroids());
                }
                bullets[i].Owner.AddScore(asteroids[j].Size);
                bullets.RemoveAt(i);
                asteroids.RemoveAt(j);
                break;
            }
        }

        // Update asteroids
        for (var i = asteroids.Count - 1; i >= 0; i--)
        {
            asteroids[i].Update();

            // Check for ship-asteroid collisions
            if (!p1Ship.IsImmune && p1Ship.IsCollidingWith(asteroids[i]))
            {
                SpawnP1Ship(p1Ship.Lives-1);
                pixelBombs.Add(new PixelBomb((int)p1Ship.Location.X, (int)p1Ship.Location.Y, 20, p1Ship.Color, 3));
                break;
            }
            // Check for ship-asteroid collisions
            if (!p2Ship.IsImmune && p2Ship.IsCollidingWith(asteroids[i]))
            {
                SpawnP2Ship(p1Ship.Lives-1);
                pixelBombs.Add(new PixelBomb((int)p2Ship.Location.X, (int)p2Ship.Location.Y, 20, p2Ship.Color, 3));
                break;
            }
        }
        
        if (p2Ship.Lives < 0 && p1Ship.Lives < 0)
        {
            State = GameOverState.Draw;
        }
        else if (p2Ship.Lives < 0)
        {
            State = GameOverState.Player1Wins;
        }
        else if (p1Ship.Lives < 0)
        {
            State = GameOverState.Player2Wins;
        }
        
        foreach (var pixelBomb in pixelBombs.ToArray())
        {
            pixelBomb.Update();
            if (pixelBomb.IsExtinguished())
            {
                pixelBombs.Remove(pixelBomb);
            }
        }

        // Check if all asteroids have been destroyed and spawn a new wave
        if (asteroids.Count == 0)
        {
            InitializeAsteroids();
        }
    }
    
    public void Draw(IDisplay display)
    {
        p1Ship.Draw(display);
        p2Ship.Draw(display);
        foreach (var bullet in bullets)
            bullet.Draw(display);
        foreach (var asteroid in asteroids)
            asteroid.Draw(display);
        foreach (var pixelBomb in pixelBombs)
            pixelBomb.Draw(display);

        // Draw scores
        scoreFont.DrawText(display, 1, 5, p1Ship.Color, $"{p1Ship.Score}/{p1Ship.Lives}L");
        scoreFont.DrawText(display, 40, 5, p2Ship.Color, $"{p2Ship.Score}/{p2Ship.Lives}L");
    }

    public GameOverState State { get; private set; }
}