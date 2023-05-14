using System.Drawing;
using Core;

namespace LunarLander.Bits;

public class LandingPad : IGameElement
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }

    public LandingPad(int x, int y)
    {
        X = x; // change this to move the landing pad
        Y = y; // change this to adjust the height of the landing pad
        Width = 8; // change this to adjust the width of the landing pad
    }

    public bool IsCollidingWith(int x, int y)
    {
        // Check for collision with landing pad
        return y == Y && x >= X && x <= X + Width;
    }

    public bool IsOnTopOf(int x, int y)
    {
        // Check if the landing pad is below the given position
        return y < Y && x >= X && x <= X + Width;
    }

    public void Update()
    {
        /*...*/
    }

    public void Draw(IDisplay display)
    {
        // Draw the landing pad
        display.DrawLine(X, Y, X + Width, Y, Color.Green);
    }
}