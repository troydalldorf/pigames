using System.Drawing;
using Core;
using Core.Display;

namespace Asteroids.Bits;

public class Ship : VectorElement
{
    private const float ThrustPower = 0.1f;
    public bool Thrusting { get; set; }
    public int Score { get; private set; }
    
    public int Lives { get; set; } = 3;
    private readonly DateTime immuneUntil;
    public bool IsImmune => immuneUntil > DateTime.Now;

    private static readonly PointF[] Shape = new[]
    {
        new PointF(-1, 1),
        new PointF(0, -1),
        new PointF(1, 1),
    };

    public Ship(int displayWidth, int displayHeight, Color color, int lives, int score) : base(Shape, 90, color, displayWidth, displayHeight)
    {
        this.Lives = lives;
        this.Score = score;
        this.immuneUntil = DateTime.Now.AddSeconds(1);
    }
    
    public void AddScore(int score)
    {
        Score += score;
    }

    public override void Update()
    {
        if (Lives < 0) return;
        if (Thrusting)
        {
            var thrustRotation = Rotation + 90;
            Velocity = new PointF(
                Velocity.X + (float)(Math.Sin(thrustRotation * Math.PI / 180) * ThrustPower), 
                Velocity.Y - (float)(Math.Cos(thrustRotation * Math.PI / 180) * ThrustPower));
        }
        base.Update();
    }

    public override void Draw(IDisplay display)
    {
        if (Lives < 0) return;
        
        // ship
        base.Draw(display);
        
        if (Thrusting)
        {
            // Calculate the direction of the ship's nose
            var radians = (Rotation + 270) * Math.PI / 180;
            var thrustDirection = new PointF((float)Math.Sin(radians), (float)-Math.Cos(radians));
            var thrusterRadius = Size * 2f;

            // Calculate the start and end points of the orange line
            var thrusterStart = new PointF(Location.X - thrustDirection.X * Size / 2, Location.Y - thrustDirection.Y * Size / 2);
            var thrusterEnd = new PointF(Location.X + thrustDirection.X * thrusterRadius, Location.Y + thrustDirection.Y * thrusterRadius);
    
            // Calculate the start and end points of the two yellow lines
            var offset = thrustDirection.Rotate(90f * (float)Math.PI / 180);
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
    }
}