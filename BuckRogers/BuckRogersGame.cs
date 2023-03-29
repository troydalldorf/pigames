using System;
using System.Drawing;
using Core;
using Core.Display.Fonts;

public class BuckRogersGame : IPlayableGameElement
{
    private const int DisplaySize = 64;
    private const int PlayerSize = 3;
    private const int ConeSize = 5;
    private const int AlienSize = 3;
    private const int InitialSpeed = 3;
    private const int MaxSpeed = 10;
    private const int SpeedIncreaseInterval = 5000; // Time in milliseconds to increase speed
    private const int AlienSpawnInterval = 2000; // Time in milliseconds to spawn alien waves

    private readonly LedFont font;

    private int _playerX;
    private int _playerY;
    private int _gameSpeed;
    private DateTime _lastSpeedIncrease;
    private DateTime _lastAlienSpawn;
    private List<Cone> _cones;
    private List<Alien> _aliens;

    public BuckRogersGame()
    {
        font = new LedFont(LedFontType.Font4x6);

        InitializeGame();
    }

    private void InitializeGame()
    {
        _playerX = DisplaySize / 2;
        _playerY = DisplaySize - 10;
        _gameSpeed = InitialSpeed;
        _lastSpeedIncrease = DateTime.UtcNow;
        _lastAlienSpawn = DateTime.UtcNow;
        _cones = new List<Cone>();
        _aliens = new List<Alien>();
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var direction = player1Console.ReadJoystick();
        var buttons = player1Console.ReadButtons();

        int dx = 0, dy = 0;
        switch (direction)
        {
            case JoystickDirection.Up:
                dy = -1;
                break;
            case JoystickDirection.Down:
                dy = 1;
                break;
            case JoystickDirection.Left:
                dx = -1;
                break;
            case JoystickDirection.Right:
                dx = 1;
                break;
        }

        var newX = _playerX + dx;
        var newY = _playerY + dy;

        if (newX >= 0 && newX < DisplaySize && newY >= 0 && newY < DisplaySize)
        {
            _playerX = newX;
            _playerY = newY;
        }
    }

    public void Update()
    {
        // Move cones
        for (int i = _cones.Count - 1; i >= 0; i--)
        {
            Cone cone = _cones[i];
            cone.Y += _gameSpeed;
            if (cone.Y >= DisplaySize)
            {
                _cones.RemoveAt(i);
            }
            else if (CheckCollision(_playerX, _playerY, PlayerSize, cone.X, cone.Y, ConeSize))
            {
                // Player collided with a cone, game over
                InitializeGame();
                return;
            }
        }

        // Move aliens
        for (int i = _aliens.Count - 1; i >= 0; i--)
        {
            Alien alien = _aliens[i];
            alien.Y += _gameSpeed;
            if (alien.Y >= DisplaySize)
            {
                _aliens.RemoveAt(i);
            }
            else if (CheckCollision(_playerX, _playerY, PlayerSize, alien.X, alien.Y, AlienSize))
            {
                InitializeGame();
                return;
            }
        }

        // Spawn new cones and aliens
        DateTime currentTime = DateTime.UtcNow;

        if ((currentTime - _lastSpeedIncrease).TotalMilliseconds >= SpeedIncreaseInterval)
        {
            _gameSpeed = Math.Min(_gameSpeed + 1, MaxSpeed);
            _lastSpeedIncrease = currentTime;
        }

        if ((currentTime - _lastAlienSpawn).TotalMilliseconds >= AlienSpawnInterval)
        {
            SpawnAlienWave();
            _lastAlienSpawn = currentTime;
        }

        if (_cones.Count < 10)
        {
            SpawnCone();
        }
    }

    private bool CheckCollision(int x1, int y1, int size1, int x2, int y2, int size2)
    {
        return Math.Abs(x1 - x2) < (size1 + size2) / 2 && Math.Abs(y1 - y2) < (size1 + size2) / 2;
    }

    private void SpawnCone()
    {
        int x = new Random().Next(DisplaySize);
        _cones.Add(new Cone(x, -ConeSize));
    }

    private void SpawnAlienWave()
    {
        int waveSize = new Random().Next(1, 5);
        int startY = -AlienSize;
        int startX = new Random().Next(DisplaySize - waveSize * AlienSize);

        for (int i = 0; i < waveSize; i++)
        {
            _aliens.Add(new Alien(startX + i * AlienSize, startY));
        }
    }

    public void Draw(IDisplay display)
    {
        display.Clear();

        // Draw player
        display.DrawRectangle(_playerX - PlayerSize / 2, _playerY - PlayerSize / 2, PlayerSize, PlayerSize, Color.White);

        // Draw cones
        foreach (var cone in _cones)
        {
            display.DrawRectangle(cone.X - ConeSize / 2, cone.Y - ConeSize / 2, ConeSize, ConeSize, Color.Green);
        }

        // Draw aliens
        foreach (var alien in _aliens)
        {
            display.DrawRectangle(alien.X - AlienSize / 2, alien.Y - AlienSize / 2, AlienSize, AlienSize, Color.Red);
        }
    }

    public GameOverState State => GameOverState.None;

    private class Cone
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Cone(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private class Alien
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Alien(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}