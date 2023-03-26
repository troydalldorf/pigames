using System.Drawing;

namespace Asteroids.Bits;

public class Ship : VectorElement
{
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
    
    private const float ShipRadius = 2.0f;
}