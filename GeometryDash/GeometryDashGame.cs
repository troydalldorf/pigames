using System.Drawing;
using Core;
using Core.Display.Fonts;

public class GeometryDashGame
{
    private LedFont _font;

    private int _playerX;
    private int _playerY;
    private int _playerSize;
    private bool _isJumping;

    private List<Obstacle> _obstacles;
    private int _obstacleSpawnInterval;
    private int _obstacleSpeed;

    private bool _isGameOver;
    private int _score;

    public GeometryDashGame()
    {
        _font = new LedFont(LedFontType.FontTomThumb);

        _playerX = _display.Width / 3;
        _playerY = _display.Height / 2;
        _playerSize = 4;

        _obstacles = new List<Obstacle>();
        _obstacleSpawnInterval = 20;
        _obstacleSpeed = 3;

        _isGameOver = false;
        _score = 0;
    }

    public void Run()
    {
        var frameCounter = 0;

        while (!_isGameOver)
        {
            _display.Clear();

            UpdatePlayer();
            UpdateObstacles();

            if (frameCounter % _obstacleSpawnInterval == 0)
            {
                SpawnObstacle();
            }

            CheckCollisions();

            DrawElements();
            _display.Update();

            Thread.Sleep(16); // Approximate 60 frames per second
            frameCounter++;
            _score++;
        }
        _display.Clear();
        //_font.DrawText(_display.Width / 2 - 30, _display.Height / 2 - 10, "Game Over", Color.Red);
        _display.Update();
    }

    private void UpdatePlayer()
    {
        var buttons = _playerConsole.ReadButtons();
        if (buttons.HasFlag(Buttons.Green) && !_isJumping)
        {
            _isJumping = true;
        }

        if (_isJumping)
        {
            // Implement jumping logic here
        }
    }

    private void UpdateObstacles()
    {
        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            _obstacles[i].X -= _obstacleSpeed;
            if (_obstacles[i].X < -_obstacles[i].Size)
            {
                _obstacles.RemoveAt(i);
            }
        }
    }

    private void SpawnObstacle()
    {
        int size = 4;
        int y = _display.Height / 2 - size;
        _obstacles.Add(new Obstacle(_display.Width, y, size));
    }

    private void CheckCollisions()
    {
        foreach (var obstacle in _obstacles)
        {
            if (Math.Abs(_playerX - obstacle.X) < _playerSize && Math.Abs(_playerY - obstacle.Y) < _playerSize)
            {
                _isGameOver = true;
            }
        }
    }

    private void DrawElements()
    {
        _display.DrawRectangle(_playerX, _playerY, _playerSize, _playerSize, Color.Blue);
        foreach (var obstacle in _obstacles)
        {
            if (obstacle.Shape == ObstacleShape.Square)
            {
                _display.DrawRectangle(obstacle.X, obstacle.Y, obstacle.Size, obstacle.Size, Color.Red, Color.Red);
            }
            else if (obstacle.Shape == ObstacleShape.Triangle)
            {
                int halfSize = obstacle.Size / 2;
                _display.DrawLine(obstacle.X, obstacle.Y + obstacle.Size, obstacle.X + halfSize, obstacle.Y, Color.Red);
                _display.DrawLine(obstacle.X + halfSize, obstacle.Y, obstacle.X + obstacle.Size, obstacle.Y + obstacle.Size, Color.Red);
                _display.DrawLine(obstacle.X + obstacle.Size, obstacle.Y + obstacle.Size, obstacle.X, obstacle.Y + obstacle.Size, Color.Red);
            }
        }

        _font.DrawText(_display, 5, 0, Color.White, $"Score: {_score}");
    }
}

public class Obstacle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Size { get; set; }
    public ObstacleShape Shape { get; set; }
    public Obstacle(int x, int y, int size)
    {
        X = x;
        Y = y;
        Size = size;
        Shape = (ObstacleShape)new Random().Next(0, 2);
    }
}

public enum ObstacleShape
{
    Square,
    Triangle
}