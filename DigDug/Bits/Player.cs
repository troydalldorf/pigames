using System.Drawing;
using Core;

namespace DigDug.Bits;

public class Player : IGameElement
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void HandleInput(JoystickDirection stick)
    {
        if (stick.HasFlag(JoystickDirection.Up)) Y -= Constants.PlayerSpeed;
        if (stick.HasFlag(JoystickDirection.Down)) Y += Constants.PlayerSpeed;
        if (stick.HasFlag(JoystickDirection.Left)) X -= Constants.PlayerSpeed;
        if (stick.HasFlag(JoystickDirection.Right)) X += Constants.PlayerSpeed;
    }

    public void Update()
    {
        // Update player state, e.g., animations or weapon cooldowns
    }

    public void Draw(IDisplay display)
    {
        display.DrawCircle(X, Y, 3, Color.Yellow); // Simplified player representation
    }
}