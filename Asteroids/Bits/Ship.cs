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
        var thrusterRotation = Rotation + 180;
        var dx = (float)(Math.Sin(thrusterRotation * Math.PI / 180) * Size);
        var dy = (float)(Math.Cos(thrusterRotation * Math.PI / 180) * Size);
        display.DrawLine((int)(Location.X+dx), (int)(Location.Y+dy), (int)(Location.X+dx*2), (int)(Location.Y+dy*2), Color.Orange);
        
        // gun
        dx = (float)(Math.Sin(Rotation * Math.PI / 180) * Size);
        dy = (float)(Math.Cos(Rotation * Math.PI / 180) * Size);
        display.SetPixel((int)(Location.X+dx), (int)(Location.Y+dy),  Color.GreenYellow);
        
        // ship
        base.Draw(display);
    }
}