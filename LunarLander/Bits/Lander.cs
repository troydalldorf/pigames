using System.Drawing;
using Core;

namespace LunarLander.Bits;

public class Lander : IGameElement
{
    private int _x;
    private int _y;
    private int _dx;
    private int _dy;

    public Lander()
    {
        _x = 32;  // start in the middle of the screen
        _y = 0;  // start at the top of the screen
        _dx = 0;  // initial horizontal speed
        _dy = 0;  // initial vertical speed
    }

    public void MoveUp()
    {
        _dy = -1;
    }

    public void MoveDown()
    {
        _dy = 1;
    }

    public void MoveLeft()
    {
        _dx = -1;
    }

    public void MoveRight()
    {
        _dx = 1;
    }

    public void Stabilize()
    {
        _dx = 0;
        _dy = 0;
    }

    public void Update()
    {
        // Move the lander according to its speed
        _x += _dx;
        _y += _dy;

        // Clamp the lander's position to the screen
        _x = Math.Max(0, Math.Min(63, _x));
        _y = Math.Max(0, Math.Min(63, _y));
    }

    public void Draw(IDisplay display)
    {
        // Draw the lander at (_x, _y) location
        display.SetPixel(_x, _y, Color.White);
    }

    public bool IsCollidingWith(Terrain terrain)
    {
        return terrain.IsCollidingWith(_x, _y);
    }

    public bool IsCollidingWith(LandingPad landingPad)
    {
        return landingPad.IsCollidingWith(_x, _y);
    }

    public bool HasLandedOn(LandingPad landingPad)
    {
        return _y == landingPad.Y - 1 && _x >= landingPad.X && _x <= landingPad.X + landingPad.Width;
    }
}