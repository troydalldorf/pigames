using Core;
using Core.Display.Fonts;

namespace BomberMan;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

public class BombermanGame : I2PGameElement
{
    private const int GridSize = 8;
    private const int BombTimer = 3000;
    private const int ExplosionDuration = 500;

    private LedFont _font;

    private Grid _grid;
    private Player _player1;
    private Player _player2;
    private List<Bomb> _bombs;
    private List<Explosion> _explosions;
    private bool _isDone;

    public BombermanGame()
    {
        _font = new LedFont(LedFontType.Font4x6);
        Initialize();
    }

    private void Initialize()
    {
        _grid = new Grid(GridSize, GridSize);
        _player1 = new Player(0, 0);
        _player2 = new Player(GridSize - 1, GridSize - 1);
        _bombs = new List<Bomb>();
        _explosions = new List<Explosion>();

        _isDone = false;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandlePlayerInput(player1Console, _player1);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandlePlayerInput(player2Console, _player2);
    }

    private void HandlePlayerInput(IPlayerConsole console, Player player)
    {
        var joystickDirection = console.ReadJoystick();
        var buttons = console.ReadButtons();

        switch (joystickDirection)
        {
            case JoystickDirection.Up:
                player.Move(0, -1);
                break;
            case JoystickDirection.Down:
                player.Move(0, 1);
                break;
            case JoystickDirection.Left:
                player.Move(-1, 0);
                break;
            case JoystickDirection.Right:
                player.Move(1, 0);
                break;
        }

        if (buttons.HasFlag(Buttons.Red))
        {
            PlaceBomb(player.X, player.Y);
        }
    }

    public void Update()
    {
        UpdateBombs();
        UpdateExplosions();
        CheckPlayerCollision();
    }

    private void UpdateBombs()
    {
        for (int i = _bombs.Count - 1; i >= 0; i--)
        {
            var bomb = _bombs[i];
            bomb.Update();

            if (bomb.IsExploded)
            {
                _bombs.RemoveAt(i);
                CreateExplosion(bomb.X, bomb.Y);
            }
        }
    }

    private void UpdateExplosions()
    {
        for (int i = _explosions.Count - 1; i >= 0; i--)
        {
            var explosion = _explosions[i];
            explosion.Update();

            if (explosion.IsDone)
            {
                _explosions.RemoveAt(i);
            }
        }
    }

    private void CheckPlayerCollision()
    {
        foreach (var explosion in _explosions)
        {
            if (explosion.CollidesWith(_player1.X, _player1.Y) || explosion.CollidesWith(_player2.X, _player2.Y))
            {
                _isDone = true;
                break;
            }
       
        }
    }

    public void Draw(IDisplay display)
    {
        _grid.Draw(display);
        _player1.Draw(display, Color.Blue);
        _player2.Draw(display, Color.Red);

        foreach (var bomb in _bombs)
        {
            bomb.Draw(display);
        }

        foreach (var explosion in _explosions)
        {
            explosion.Draw(display);
        }

        if (_isDone)
        {
            _font.DrawText(display, 10, 30, Color.White, "Game Over");
        }
    }

    public GameOverState State => _isDone ? GameOverState.EndOfGame : GameOverState.None;

    private void PlaceBomb(int x, int y)
    {
        _bombs.Add(new Bomb(x, y, BombTimer));
    }

    private void CreateExplosion(int x, int y)
    {
        _explosions.Add(new Explosion(x, y, ExplosionDuration));
    }
}

public class Grid
{
    private int _width;
    private int _height;
    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void Draw(IDisplay display)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                display.DrawRectangle(x * 8, y * 8, 8, 8, Color.Gray);
            }
        }
    }
}

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Player(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(int dx, int dy)
    {
        X += dx;
        Y += dy;
    }

    public void Draw(IDisplay display, Color color)
    {
        display.DrawRectangle(X * 8, Y * 8, 8, 8, color, color);
    }
}

public class Bomb
{
    public int X { get; }
    public int Y { get; }
    public bool IsExploded => _timer.ElapsedMilliseconds >= _timeout;
    private Timer _timer;
    private int _timeout;

    public Bomb(int x, int y, int timeout)
    {
        X = x;
        Y = y;
        _timeout = timeout;
        _timer = new Timer();
        _timer.Start();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.DrawCircle(X * 8 + 4, Y * 8 + 4, 4, Color.Black);
    }
}

public class Explosion
{
    public int X { get; }
    public int Y { get; }
    public bool IsDone => _timer.ElapsedMilliseconds >= _duration;
    private Timer _timer;
    private int _duration;

    public Explosion(int x, int y, int duration)
    {
        X = x;
        Y = y;
        _duration = duration;
        _timer = new Timer();
        _timer.Start();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(X * 8, Y * 8, 8, 8, Color.Orange, Color.Orange);
    }

    public bool CollidesWith(int x, int y)
    {
        return X == x && Y == y;
    }
}

public class Timer
{
    private DateTime _startTime;

    public Timer()
    {
        _startTime = DateTime.Now;
    }

    public void Start()
    {
        _startTime = DateTime.Now;
    }

    public long ElapsedMilliseconds => (long)(DateTime.Now - _startTime).TotalMilliseconds;
}