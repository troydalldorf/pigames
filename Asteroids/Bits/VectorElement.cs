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
    private PointF[] shape;
    
    public VectorElement(PointF[] shape)
    {
        this.shape = shape;
    }
    
    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Draw(IDisplay display)
    {
        // Scale and rotate the shape
        var transformedShape = new PointF[shape.Length];
        for (var i = 0; i < shape.Length; i++)
        {
            var x = shape[i].X * Size;
            var y = shape[i].Y * Size;

            // Rotate the point
            var rotatedX = x * (float)Math.Cos(Rotation) - y * (float)Math.Sin(Rotation);
            var rotatedY = x * (float)Math.Sin(Rotation) + y * (float)Math.Cos(Rotation);

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
                Color.White
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