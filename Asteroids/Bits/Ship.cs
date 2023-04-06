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

    public Ship(int displayWidth, int displayHeight, Color color) : base(Shape, 90f, color, displayWidth, displayHeight)
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
        if (Thrusting)
        {
            // Calculate the direction of the ship's nose
            var radians = Rotation * Math.PI / 180;
            var noseDirection = new PointF((float)Math.Sin(radians), (float)-Math.Cos(radians));

            // Calculate the start and end points of the orange line
            var thrusterStart = new PointF(Location.X - noseDirection.X * Size / 2, Location.Y - noseDirection.Y * Size / 2);
            var thrusterEnd = new PointF(Location.X + noseDirection.X * Size / 2, Location.Y + noseDirection.Y * Size / 2);
        
            // Calculate the start and end points of the two yellow lines
            var offset = noseDirection.Rotate(-90);
            offset = new PointF(offset.X * Size / 4, offset.Y * Size / 4);
            var leftOffset = new PointF(thrusterEnd.X - offset.X, thrusterEnd.Y - offset.Y);
            var rightOffset = new PointF(thrusterEnd.X + offset.X, thrusterEnd.Y + offset.Y);
        
            // Draw the thruster flames
            display.DrawLine(thrusterStart, thrusterEnd, Color.Orange);
            display.DrawLine(thrusterEnd, leftOffset, Color.Yellow);
            display.DrawLine(thrusterEnd, rightOffset, Color.Yellow);
        }
        // gun
        display.SetPixel((int)(Location.X+Velocity.X), (int)(Location.Y+Velocity.X),  Color.WhiteSmoke);
        
        // ship
        base.Draw(display);
    }
}