using System.Drawing;
using Core;
using Core.Fonts;

public class MazeGame : IDuoPlayableGameElement
{
    private Maze.Bits.Maze maze;
    private int level;
    private Point player1Position;
    private Point player2Position;
    private readonly IFont font;

    public MazeGame(IFontFactory fontFactory)
    {
        level = 1;
        InitializeLevel();
        font = fontFactory.GetFont(LedFontType.Font5x7);
    }

    private void InitializeLevel()
    {
        maze = new Maze.Bits.Maze(6 + level * 2, 6 + level * 2);
        // Set the start and exit points for players
        player1Position = new Point(1, 1);
        player2Position = new Point(maze.Width - 2, maze.Height - 2);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var direction = player1Console.ReadJoystick();
        MovePlayer(ref player1Position, direction);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var direction = player2Console.ReadJoystick();
        MovePlayer(ref player2Position, direction);
    }

    private void MovePlayer(ref Point position, JoystickDirection direction)
    {
        Point newPosition = position;
        switch (direction)
        {
            case JoystickDirection.Up:
                newPosition.Y -= 1;
                break;
            case JoystickDirection.Down:
                newPosition.Y += 1;
                break;
            case JoystickDirection.Left:
                newPosition.X -= 1;
                break;
            case JoystickDirection.Right:
                newPosition.X += 1;
                break;
        }

        if (!maze.IsWall(newPosition.X, newPosition.Y))
        {
            position = newPosition;
        }
    }

    public void Update()
    {
        // Check if players reached their destination
        if (player1Position == new Point(maze.Width - 1, maze.Height - 1) ||
            player2Position == new Point(1, 1))
        {
            level++;
            InitializeLevel();
        }
    }

    public void Draw(IDisplay display)
    {
        var xOffset = (display.Width - maze.Width) / 2;
        var yOffset = (display.Height - maze.Height) / 2;
        display.DrawRectangle(xOffset, yOffset, maze.Width+1, maze.Height+1, Color.White);
        for (var x = 0; x < maze.Width; x++)
        {
            for (var y = 0; y < maze.Height; y++)
            {
                if (maze.IsWall(x, y))
                {
                    display.SetPixel(xOffset + x, yOffset + y, Color.White);
                }
            }
        }

        // Draw players
        display.SetPixel(xOffset + player1Position.X, yOffset + player1Position.Y, Color.Red);
        display.SetPixel(xOffset + player2Position.X, yOffset + player2Position.Y, Color.Blue);

        // Draw level information
        font.DrawText(display, 0, 60, Color.Green, $"Level: {level}");
    }

    public GameOverState State
    {
        get { return GameOverState.None; } // You can add a condition to end the game and change the state accordingly
    }
}
