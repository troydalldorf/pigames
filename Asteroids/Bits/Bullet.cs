using System.Drawing;
using Core;

namespace Asteroids.Bits;

public class Bullet : IGameElement
{
    public PointF Location { get; private set; }
    private PointF lastLocation;
    public PointF Velocity { get; }
    public int Life { get; }
    
    public Bullet(PointF location, PointF velocity, int life)
    {
        Location = location;
        lastLocation = location;
        Velocity = velocity;
        Life = life;
    }
    
    public void Update()
    {
        lastLocation = Location;
        Location = new PointF(Location.X + Velocity.X, Location.Y + Velocity.Y);
    }

    public void Draw(IDisplay display)
    {
        display.DrawLine((int)lastLocation.X, (int)lastLocation.Y, (int)Location.X, (int)Location.Y, Color.White);
    }
    
    private const float BulletRadius = 2f;

    public bool IsCollidingWith(Asteroid asteroid)
    {
        var asteroidRadius = asteroid.Size / 2.0f;
        var distanceSquared = (Location.X - asteroid.Location.X) * (Location.X - asteroid.Location.X)
                              + (Location.Y - asteroid.Location.Y) * (Location.Y - asteroid.Location.Y);
        var sumOfRadiiSquared = (BulletRadius + asteroidRadius) * (BulletRadius + asteroidRadius);
        return distanceSquared <= sumOfRadiiSquared;
    }
}