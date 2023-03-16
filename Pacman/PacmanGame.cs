using System.Drawing;
using Core.Display;
using Core.Inputs;
using Pacman;

public class PacManGame
{
    private readonly LedDisplay display;
    private readonly PlayerConsole playerConsole;
    private PacMan pacMan;
    private List<Ghost> ghosts;
    private Maze maze;

    public PacManGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        pacMan = new PacMan(32, 32, 1);
        ghosts = new List<Ghost>
        {
            new Ghost(16, 16, 1),
            new Ghost(48, 16, 1),
            new Ghost(16, 48, 1),
            new Ghost(48, 48, 1)
        };
        maze = new Maze();
    }

    public void Run()
    {
        while (true)
        {
            Update();
            Thread.Sleep(33); // Limit to 30 FPS
        }
    }

    private void Update()
    {
        var stick = playerConsole.ReadJoystick();
        if (stick.IsUp() && !maze.IsWall(pacMan.X, pacMan.Y - pacMan.Speed))
        {
            pacMan.Direction = Direction.Up;
        }
        else if (stick.IsDown() && !maze.IsWall(pacMan.X, pacMan.Y + pacMan.Speed))
        {
            pacMan.Direction = Direction.Down;
        }
        else if (stick.IsLeft() && !maze.IsWall(pacMan.X - pacMan.Speed, pacMan.Y))
        {
            pacMan.Direction = Direction.Left;
        }
        else if (stick.IsRight() && !maze.IsWall(pacMan.X + pacMan.Speed, pacMan.Y))
        {
            pacMan.Direction = Direction.Right;
        }

        pacMan.Move();
        // Update ghost positions, game logic, and collision detection as needed.

        Draw();
    }

    private void Draw()
    {
        display.Clear();
        maze.Draw(display);
        // Draw Pac-Man (yellow circle)
        display.DrawCircle(pacMan.X, pacMan.Y, 2, Color.Yellow);

        // Draw ghosts (different colors)
        Color[] ghostColors = { Color.Red, Color.Magenta, Color.Cyan, Color.Orange };
        for (int i = 0; i < ghosts.Count; i++)
        {
            Ghost ghost = ghosts[i];
            display.DrawCircle(ghost.X, ghost.Y, 2, ghostColors[i]);
        }

        display.Update();
    }
}
