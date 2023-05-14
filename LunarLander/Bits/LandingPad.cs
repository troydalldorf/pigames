using System.Drawing;
using Core;

namespace LunarLander.Bits;

public class LandingPad : IGameElement
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _width;

    public LandingPad()
    {
        _x = 28; // change this to move the landing pad
        _y = 50; // change this to adjust the height of the landing pad
        _width = 8; // change this to adjust the width of the landing pad
    }

    public bool IsCollidingWith(int x, int y)
    {
        // Check for collision with landing pad
        return y == _y && x >= _x && x <= _x + _width;
    }

    public bool IsOnTopOf(int x, int y)
    {
        // Check if the landing pad is below the given position
        return y < _y && x >= _x && x <= _x + _width;
    }

    public void Update()
    {
        /*...*/
    }

    public void Draw(IDisplay display)
    {
        // Draw the landing pad
        display.DrawLine(_x, _y, _x + _width, _y, Color.Green);
    }
}