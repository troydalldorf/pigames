using Core;
using Core.Display.Fonts;

namespace BomberMan;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

public class BombermanGame : IDuoGameElement
{
    private const int GridSize = 8;
    private const int BombTimer = 3000;
    private const int ExplosionDuration = 500;
    private readonly LedFont font;

    private Grid grid;
    private Player player1;
    private Player player2;
    private List<Bomb> bombs;
    private List<Explosion> explosions;
    private bool isDone;

    public BombermanGame()
    {
        font = new LedFont(LedFontType.Font4x6);
        Initialize();
    }

    private void Initialize()
    {
        grid = new Grid(GridSize, GridSize);
        player1 = new Player(0, 0, grid);
        player2 = new Player(GridSize - 1, GridSize - 1, grid);
        bombs = new List<Bomb>();
        explosions = new List<Explosion>();

        isDone = false;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandlePlayerInput(player1Console, player1);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandlePlayerInput(player2Console, player2);
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

        if (buttons.HasFlag(Buttons.Green))
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
        for (int i = bombs.Count - 1; i >= 0; i--)
        {
            var bomb = bombs[i];
            bomb.Update();

            if (bomb.IsExploded)
            {
                bombs.RemoveAt(i);
                CreateExplosion(bomb.X, bomb.Y);
            }
        }
    }

    private void UpdateExplosions()
    {
        for (int i = explosions.Count - 1; i >= 0; i--)
        {
            var explosion = explosions[i];
            explosion.Update();

            if (explosion.IsDone)
            {
                explosions.RemoveAt(i);
            }
        }
    }

    private void CheckPlayerCollision()
    {
        foreach (var explosion in explosions)
        {
            if (explosion.CollidesWith(player1.X, player1.Y) || explosion.CollidesWith(player2.X, player2.Y))
            {
                isDone = true;
                break;
            }
       
        }
    }

    public void Draw(IDisplay display)
    {
        grid.Draw(display);
        player1.Draw(display, Color.Blue);
        player2.Draw(display, Color.Red);

        foreach (var bomb in bombs)
        {
            bomb.Draw(display);
        }

        foreach (var explosion in explosions)
        {
            explosion.Draw(display);
        }

        if (isDone)
        {
            font.DrawText(display, 10, 30, Color.White, "Game Over");
        }
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;

    private void PlaceBomb(int x, int y)
    {
        bombs.Add(new Bomb(x, y, BombTimer));
    }

    private void CreateExplosion(int x, int y)
    {
        explosions.Add(new Explosion(x, y, ExplosionDuration));
    }
}

public class Grid
{
    private bool[,] _maze;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _maze = new bool[width, height];
        GenerateMaze();
    }
    
    public int Width { get; private set; }
    public int Height { get; private set; }

    private void GenerateMaze()
    {
        // Hard-coded maze layout (1 for walls, 0 for paths)
        int[,] mazeLayout =
        {
            { 1, 0, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 1 }
        };
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _maze[x, y] = mazeLayout[y, x] == 1;
            }
        }
    }

    public bool IsWall(int x, int y)
    {
        return _maze[x, y];
    }

    public void Draw(IDisplay display)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (_maze[x, y])
                {
                    display.DrawRectangle(x * 8, y * 8, 8, 8, Color.Gray, Color.Gray);
                }
            }
        }
    }
}

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private Grid _grid;

    public Player(int x, int y, Grid grid)
    {
        X = x;
        Y = y;
        _grid = grid;
    }

    public void Move(int dx, int dy)
    {
        int newX = X + dx;
        int newY = Y + dy;
        if (newX >= 0 && newX < _grid.Width && newY >= 0 && newY < _grid.Height && !_grid.IsWall(newX, newY))
        {
            X = newX;
            Y = newY;
        }
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