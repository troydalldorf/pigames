using System.Drawing;
using Core;

namespace Asteroids.Bits;

public class Asteroid : VectorElement
{
    private static readonly Random Random = new Random();
    private const int NumSmallerAsteroids = 2;

    private static readonly List<PointF[]> ShapeVariants = new()
    {
        new[] { new PointF(-1, -1), new PointF(1, -1), new PointF(1, 1), new PointF(-1, 1) },
        new[] { new PointF(-1, 1), new PointF(-0.5f, -1), new PointF(0.5f, -1), new PointF(1, 1), new PointF(0, 0.5f) },
        new[] { new PointF(-1, 0), new PointF(0, -1), new PointF(1, 0), new PointF(0, 1) },
    };

    public Asteroid(int displayWidth, int displayHeight) : base(GetRandomShape(), 0f, Color.Gray, displayWidth, displayHeight)
    {
    }

    public IEnumerable<Asteroid> SpawnSmallerAsteroids()
    {
        var smallerAsteroids = new List<Asteroid>();

        if (Size <= 1)
        {
            return smallerAsteroids;
        }

        for (var i = 0; i < NumSmallerAsteroids; i++)
        {
            var angle = (float)(2 * Math.PI * i / NumSmallerAsteroids);
            var newVelocity = new PointF((float)(Velocity.X + Math.Cos(angle)), (float)(Velocity.Y + Math.Sin(angle)));

            smallerAsteroids.Add(new Asteroid(DisplayWidth, DisplayHeight)
            {
                Location = new PointF(Location.X, Location.Y),
                Rotation = 0,
                Velocity = newVelocity,
                RotationSpeed = RotationSpeed,
                Size = Size / 2,
            });
        }

        return smallerAsteroids;
    }

    private static PointF[] GetRandomShape()
    {
        var variant = Random.Next(0, ShapeVariants.Count);
        return ShapeVariants[variant];
    }
}