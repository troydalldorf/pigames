using System.Drawing;
using Core;

namespace Asteroids.Bits;

public class Ship : VectorElement
{
    private const float ThrustPower = 0.1f;
    public bool Thrusting { get; set; }
    
    private static readonly PointF[] Shape = new[]
    {
        new PointF(-1, 1),
        new PointF(0, -1),
        new PointF(1, 1),
    };

    public Ship(int displayWidth, int displayHeight, Color color) : base(Shape, color, displayWidth, displayHeight)
    {
    }

    public override void Update()
    {
        if (Thrusting)
        {
            Velocity = new PointF(
                Velocity.X + (float)(Math.Sin(Rotation * Math.PI / 180) * ThrustPower), 
                Velocity.Y + (float)(Math.Cos(Rotation * Math.PI / 180) * ThrustPower));
        }
        base.Update();
    }

    public override void Draw(IDisplay display)
    {
        // thruster
        display.DrawLine((int)(Location.X-Velocity.X), (int)(Location.Y-Velocity.Y), (int)(Location.X+Velocity.Y*2), (int)(Location.Y+Velocity.Y*2), Color.Orange);
        // gun
        display.SetPixel((int)(Location.X+Velocity.X), (int)(Location.Y+Velocity.X),  Color.WhiteSmoke);
        // ship
        base.Draw(display);
    }
}