using System.Drawing;
using Core;

namespace Asteroids.Bits;

public class Bullet : IGameElement
{
    public PointF Location { get; set; }
    public PointF Velocity { get; set; }
    public int Life { get; set; }
    
    public void Update()
    {
        // Update the bullet's location based on its velocity
        Location = new PointF(Location.X + Velocity.X, Location.Y + Velocity.Y);
    }

    public void Draw(IDisplay display)
    {
        // Draw the bullet as a single pixel on the display
        display.SetPixel((int)Location.X, (int)Location.Y, Color.White);
    }
    
    private const float BulletRadius = 0.5f;

    public bool IsCollidingWith(Asteroid asteroid)
    {
        var asteroidRadius = asteroid.Size / 2.0f;
        var distanceSquared = (Location.X - asteroid.Location.X) * (Location.X - asteroid.Location.X)
                              + (Location.Y - asteroid.Location.Y) * (Location.Y - asteroid.Location.Y);
        var sumOfRadiiSquared = (BulletRadius + asteroidRadius) * (BulletRadius + asteroidRadius);
        return distanceSquared <= sumOfRadiiSquared;
    }
}