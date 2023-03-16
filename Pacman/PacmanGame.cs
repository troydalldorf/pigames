using System.Drawing;
using Core.Display;
using Core.Inputs;

namespace Pacman;

public class PacManGame
{
    private readonly LedDisplay display;
    private readonly PlayerConsole playerConsole;
    private PacMan pacMan;
    private List<Ghost> ghosts;
    // Add the game board, collision detection, and other logic as needed.

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
        if (stick.IsUp())
        {
            pacMan.Direction = Direction.Up;
        }
        else if (stick.IsDown())
        {
            pacMan.Direction = Direction.Down;
        }
        else if (stick.IsLeft())
        {
            pacMan.Direction = Direction.Left;
        }
        else if (stick.IsRight())
        {
            pacMan.Direction = Direction.Right;
        }

        pacMan.Move();
        // Update ghost positions and game logic as needed.

        Draw();
    }

    private void Draw()
    {
        display.Clear();

        // Draw game board (simplified example with a basic border)
        display.DrawRectangle(0, 0, 64, 64, Color.White);

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