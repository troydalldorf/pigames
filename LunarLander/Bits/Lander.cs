using System.Drawing;
using Core;

namespace LunarLander.Bits;

public class Lander : IGameElement
{
    private double _x;
    private double _y;
    private double _dx;
    private double _dy;

    private const double Gravity = 0.05; // Gravity constant
    private const double ThrustPower = 0.15; // Power of the thruster
    private const double Inertia = 0.98; // Inertia factor

    public Lander()
    {
        _x = 32; // start in the middle of the screen
        _y = 0; // start at the top of the screen
        _dx = 0; // initial horizontal speed
        _dy = 0; // initial vertical speed
    }

    public void ThrustUp()
    {
        _dy -= ThrustPower;
    }

    public void ThrustLeft()
    {
        _dx -= ThrustPower;
    }

    public void ThrustRight()
    {
        _dx += ThrustPower;
    }

    public void Update()
    {
        // Apply gravity
        _dy += Gravity;

        // Apply inertia
        _dx *= Inertia;

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
        display.SetPixel((int)_x, (int)_y, Color.White);
    }

    public bool IsCollidingWith(Terrain terrain)
    {
        return terrain.IsCollidingWith((int)_x, (int)_y);
    }

    public bool IsCollidingWith(LandingPad landingPad)
    {
        return landingPad.IsCollidingWith((int)_x, (int)_y);
    }

    public bool HasLandedOn(LandingPad landingPad)
    {
        return _y == landingPad.Y - 1 && _x >= landingPad.X && _x <= landingPad.X + landingPad.Width;
    }
}