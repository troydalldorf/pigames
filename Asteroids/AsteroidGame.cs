using System.Drawing;
using Asteroids.Bits;
using Core;
using Core.Display.Fonts;
using Core.Effects;
using Core.Fonts;

namespace Asteroids;

public class AsteroidsGame : IDuoPlayableGameElement
{
    private readonly List<Ship> ships;
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
        ships = new List<Ship>
        {
            new Ship(DisplayWidth, DisplayHeight, Color.Red) { Location = new PointF(32, 27), Size=2, Rotation = 0, Velocity = new PointF(0, 0), RotationSpeed = 0, Thrusting = false },
            new Ship(DisplayWidth, DisplayHeight, Color.Blue) { Location = new PointF(32, 37), Size=2, Rotation = 180, Velocity = new PointF(0, 0), RotationSpeed = 0, Thrusting = false }
        };
        asteroids = new List<Asteroid>();
        bullets = new List<Bullet>();
        scoreFont = new FontFactory().GetFont(LedFontType.FontTomThumb);
        State = GameOverState.None;
        InitializeAsteroids();
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
        HandlePlayerInput(player1Console, ships[0]);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandlePlayerInput(player2Console, ships[1]);
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
                BulletLife));
        }
    }

    public void Update()
    {
        foreach (var ship in ships)
        {
            ship.Update();
        }

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
            foreach (var ship in ships.Where(ship => ship.IsCollidingWith(asteroids[i])))
            {
                // Handle ship destruction and respawn
                // You can implement lives and game over logic here
                //ship.Respawn();
                pixelBombs.Add(new PixelBomb((int)ship.Location.X, (int)ship.Location.Y, 20, ship.Color, 3));
                break;
            }
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
        foreach (var ship in ships)
            ship.Draw(display);
        foreach (var bullet in bullets)
            bullet.Draw(display);
        foreach (var asteroid in asteroids)
            asteroid.Draw(display);
        foreach (var pixelBomb in pixelBombs)
            pixelBomb.Draw(display);

        // Draw scores
        scoreFont.DrawText(display, 1, 5, ships[0].Color, $"{ships[0].Score}", 1);
        scoreFont.DrawText(display, 55, 5, ships[1].Color, $"{ships[1].Score}", 1);
    }

    public GameOverState State { get; }
}