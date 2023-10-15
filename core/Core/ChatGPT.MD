I have a Raspberry PI arcade that uses a 64x64 LED matrix display. Players sit opposite each other sharing the same screen.
The players have a simple joystick, and four buttons. The buttons can also be lit up for effects. There is also a font library that
provides the ability to write text.
The code/classes used to interact with the custom arcade console are described below.
NOTE that the main game loop is outside of this code, and not something you need to provide.

```csharp
public interface IDisplay
{
    int Width { get; }
    int Height { get; }
    void Clear();
    void Update(int? frameIntervalMs);
    void SetPixel(int x, int y, Color color);
    void DrawCircle(int x, int y, int radius, Color color, Color? fillColor = null);
    void DrawLine(int x0, int y0, int x1, int y1, Color color);
    void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null);
}

public interface IPlayerConsole
{
    JoystickDirection ReadJoystick();
    Buttons ReadButtons();
    void LightButtons(bool red, bool green, bool blue, bool yellow);
}

public enum JoystickDirection
{
    None        = 0x00,
    Up          = 0x01,
    Down        = 0x02,
    Left        = 0x04,
    Right       = 0x08,
}

public enum Buttons
{
    None = 0x0,
    Red = 0x1,
    Yellow = 0x2,
    Green = 0x4,
    Blue = 0x8,
}

public interface IFontFactory
{
    IFont GetFont(LedFontType fontType);
}

public interface IFont
{
    void DrawText(IDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false);
}

public enum LedFontType
{
    Font4x6,
    Font5x7,
    Font5x8
}
```

Games will implement either `IPlayableGameElement` for single player games, or `IDuoPlayableGameElement` for two player games.
Game Elements  _should_ implement `IGameElement` and called from the main game class. 

```csharp
public interface IGameElement
{
    void Update();
    void Draw(IDisplay display);
}

public interface IPlayableGameElement : IGameElement
{
    void HandleInput(IPlayerConsole player1Console);
    GameOverState State { get; }
}

public interface IDuoPlayableGameElement : IPlayableGameElement
{
    void Handle2PInput(IPlayerConsole player2Console);
}
```

A sample game is provided below for reference purposes. Keep in mind that the main game loop is controlled outside of this
code. Control is yielded back to the main function using the `State` property.

```csharp
public class SnakeGame2P : IDuoPlayableGameElement
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private const int Width = 64;
    private const int Height = 64;
    private const int SnakeSize = 2;
    private const int InitialSnakeLength = 5;

    private readonly List<Point> snake1;
    private Direction snakeDirection1;
    private Point food1;

    private readonly List<Point> snake2;
    private Direction snakeDirection2;
    private Point food2;

    public SnakeGame2P()
    {
        snake1 = new List<Point>();
        snake2 = new List<Point>();
        InitializeSnake(snake1, Direction.Down);
        InitializeSnake(snake2, Direction.Down);
        SpawnFood(ref food1);
        SpawnFood(ref food2);
        State = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandleInput(player1Console, ref snakeDirection1);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandleInput(new PlayerConsoleInversionDecorator(player2Console), ref snakeDirection2);
    }
    
    private static void HandleInput(IPlayerConsole playerConsole, ref Direction snakeDirection)
    {
        var stick = playerConsole.ReadJoystick();

        if (stick.IsLeft() && snakeDirection != Direction.Right)
            snakeDirection = Direction.Left;
        else if (stick.IsRight() && snakeDirection != Direction.Left)
            snakeDirection = Direction.Right;
        else if (stick.IsUp() && snakeDirection != Direction.Down)
            snakeDirection = Direction.Up;
        else if (stick.IsDown() && snakeDirection != Direction.Up)
            snakeDirection = Direction.Down;
    }

    private void InitializeSnake(List<Point> snake, Direction direction)
    {
        snake.Clear();
        if (direction == Direction.Left)
        {
            for (var i = 0; i < InitialSnakeLength; i++)
            {
                snake.Add(new Point(Width - i * SnakeSize - SnakeSize, Height / 2));
            }
        }
        else
        {
            for (var i = 0; i < InitialSnakeLength; i++)
            {
                snake.Add(new Point(i * SnakeSize, Height / 2));
            }
        }

        snakeDirection1 = direction;
    }

    private static void SpawnFood(ref Point food)
    {
        var random = new Random();
        var x = random.Next(0, Width / SnakeSize) * SnakeSize;
        var y = random.Next(0, Height / SnakeSize) * SnakeSize;
        food = new Point(x, y);
    }

    public void Update()
    {
        UpdateSnake(snake1, snakeDirection1, ref food1);
        UpdateSnake(snake2, snakeDirection2, ref food2);
        var s1Collision = CheckCollision(snake1);
        var s2Collision = CheckCollision(snake2);
        if (s1Collision && s2Collision)
            State = GameOverState.Draw;
        else if (s1Collision)
            State = GameOverState.Player2Wins;
        else if (s2Collision)
            State = GameOverState.Player1Wins;
    }

    private static void UpdateSnake(IList<Point> snake, Direction snakeDirection, ref Point food)
    {
        var newPosition = new Point(snake[0].X, snake[0].Y);

        switch (snakeDirection)
        {
            case Direction.Up:
                newPosition.Y -= SnakeSize;
                break;
            case Direction.Down:
                newPosition.Y += SnakeSize;
                break;
            case Direction.Left:
                newPosition.X -= SnakeSize;
                break;
            case Direction.Right:
                newPosition.X += SnakeSize;
                break;
        }

        // If the snake eats the food
        if (newPosition == food)
        {
            snake.Insert(0, newPosition);
            SpawnFood(ref food);
        }
        else
        {
            // Move the snake by removing the tail and adding the new position as the head
            snake.RemoveAt(snake.Count - 1);
            snake.Insert(0, newPosition);
        }
    }

    private bool CheckCollision(List<Point> snake)
    {
        // Check collision with the wall
        if (snake[0].X is < 0 or >= Width || snake[0].Y is < 0 or >= Height)
        {
            return true;
        }

        // Check collision with itself
        for (var i = 1; i < snake.Count; i++)
        {
            if (snake[0] != snake[i]) continue;
            return true;
        }

        return false;
    }

    public void Draw(IDisplay display)
    {
        Draw(display, snake1, food1, Color.Green, Color.Red);
        Draw(display, snake2, food2, Color.Blue, Color.Yellow);
    }

    public GameOverState State { get; private set; }

    private static void Draw(IDisplay display, List<Point> snake, Point food, Color snakeColor, Color foodColor)
    {
        // Draw snake
        foreach (var point in snake)
        {
            display.DrawRectangle(point.X, point.Y, SnakeSize, SnakeSize, snakeColor);
        }

        // Draw food
        display.DrawRectangle(food.X, food.Y, SnakeSize, SnakeSize, foodColor);
    }
}
```