using System.Drawing;

namespace Core;

public static class DisplayExtensions
{
    public static void DrawLine(this IDisplay @this, PointF p1, PointF p2, Color color)
    {
        @this.DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color);
    }
    
    public static void DrawRectangle(this IDisplay @this, PointF p1, PointF p2, Color color)
    {
        @this.DrawRectangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color);
    }
}