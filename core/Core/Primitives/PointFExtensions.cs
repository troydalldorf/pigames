using System.Drawing;

namespace Core.Primitives;

public static class PointFExtensions
{
    public static PointF Rotate(this PointF point, float angleInRadians)
    {
        var sin = (float)Math.Sin(angleInRadians);
        var cos = (float)Math.Cos(angleInRadians);
        return new PointF(
            point.X * cos - point.Y * sin,
            point.X * sin + point.Y * cos
        );
    }
}