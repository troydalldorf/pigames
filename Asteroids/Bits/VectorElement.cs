using System.Drawing;
using Core;

namespace Asteroids.Bits;

public abstract class VectorElement : IGameElement
{
    public PointF Location { get; set; }
    public float Rotation { get; set; }
    public PointF Velocity { get; set; }
    public float RotationSpeed { get; set; }
    public int Size { get; set; }
    private readonly PointF[] shape;
    private readonly float shapeRotation;
    public Color Color { get; }
    protected int DisplayWidth { get; }
    protected int DisplayHeight { get; }

    public VectorElement(PointF[] shape, float shapeRotation, Color color, int displayWidth, int displayHeight)
    {
        this.shape = shape;
        this.shapeRotation = shapeRotation;
        this.Color = color;
        this.DisplayWidth = displayWidth;
        this.DisplayHeight = displayHeight;
    }

    public virtual void Update()
    {
        // Update location
        Location = new PointF(
            Location.X + Velocity.X,
            Location.Y + Velocity.Y
        );

        // Update rotation
        Rotation += RotationSpeed;

        // Wrap around the screen
        if (Location.X < 0)
        {
            Location = new PointF(DisplayWidth, Location.Y);
        }
        else if (Location.X > DisplayWidth)
        {
            Location = new PointF(0, Location.Y);
        }

        if (Location.Y < 0)
        {
            Location = new PointF(Location.X, DisplayHeight);
        }
        else if (Location.Y > DisplayHeight)
        {
            Location = new PointF(Location.X, 0);
        }
    }

    public virtual void Draw(IDisplay display)
    {
        // Scale and rotate the shape
        var radians = (Rotation + shapeRotation) * Math.PI / 180;
        var transformedShape = new PointF[shape.Length];
        for (var i = 0; i < shape.Length; i++)
        {
            var x = shape[i].X * Size;
            var y = shape[i].Y * Size;

            // Rotate the point
            var rotatedX = x * (float)Math.Cos(radians) - y * (float)Math.Sin(radians);
            var rotatedY = x * (float)Math.Sin(radians) + y * (float)Math.Cos(radians);

            // Translate the point
            var translatedX = rotatedX + Location.X;
            var translatedY = rotatedY + Location.Y;

            transformedShape[i] = new PointF(translatedX, translatedY);
        }

        // Draw the asteroid shape using lines
        for (var i = 0; i < transformedShape.Length; i++)
        {
            var nextIndex = (i + 1) % transformedShape.Length;
            display.DrawLine(
                (int)transformedShape[i].X, (int)transformedShape[i].Y,
                (int)transformedShape[nextIndex].X, (int)transformedShape[nextIndex].Y,
                Color
            );
        }
    }

    public bool IsCollidingWith(VectorElement other)
    {
        var thisRadius = Size / 2.0f;
        var otherRadius = other.Size / 2.0f;
        var distanceSquared = (Location.X - other.Location.X) * (Location.X - other.Location.X)
                              + (Location.Y - other.Location.Y) * (Location.Y - other.Location.Y);

        var sumOfRadiiSquared = (thisRadius + otherRadius) * (thisRadius + otherRadius);

        return distanceSquared <= sumOfRadiiSquared;
    }
}