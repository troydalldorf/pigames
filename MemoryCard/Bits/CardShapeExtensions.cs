using System.Drawing;
using Core;

namespace MemoryCard.Bits;

public static class CardShapeExtensions
{
    public static void Draw(this CardShape shape, IDisplay display, int x, int y, int size)
    {
        var color = Color.LightSlateGray;
        x += 1;
        y += 1;
        size -= 2;
        switch (shape)
        {
            case CardShape.Circle:
                color = Color.Orange;
                display.DrawCircle(x + size / 2, y + size / 2, size / 2, color);
                break;
            case CardShape.Triangle:
                color = Color.GreenYellow;
                display.DrawLine(x, y + size, x + size / 2, y, color);
                display.DrawLine(x + size / 2, y, x + size, y + size, color);
                display.DrawLine(x + size, y + size, x, y + size, color);
                break;
            case CardShape.Square:
                color = Color.Blue;
                display.DrawRectangle(x, y, size, size, color, color);
                break;
            case CardShape.Diamond:
                color = Color.White;
                display.DrawLine(x + size / 2, y, x, y + size / 2, color);
                display.DrawLine(x, y + size / 2, x + size / 2, y + size, color);
                display.DrawLine(x + size / 2, y + size, x + size, y + size / 2, color);
                display.DrawLine(x + size, y + size / 2, x + size / 2, y, color);
                break;
            case CardShape.Star:
                color = Color.Yellow;
                DrawStar(x, y, size, display, color);
                break;
            case CardShape.Plus:
                color = Color.Red;
                var lineWidth = size / 4;
                display.DrawRectangle(x + lineWidth, y, lineWidth, size, color, color);
                display.DrawRectangle(x, y + lineWidth, size, lineWidth, color, color);
                break;
            case CardShape.Cross:
                color = Color.Gold;
                display.DrawLine(x, y, x + size, y + size, color);
                display.DrawLine(x + size, y, x, y + size, color);
                break;
            case CardShape.Hexagon:
                color = Color.Magenta;
                var xOffset = size / 4;
                var yOffset = size / 2;
                display.DrawLine(x + xOffset, y, x + size - xOffset, y, color);
                display.DrawLine(x, y + yOffset / 2, x + xOffset, y, color);
                display.DrawLine(x, y + yOffset * 3 / 2, x + xOffset, y + yOffset * 2, color);
                display.DrawLine(x + size, y + yOffset / 2, x + size - xOffset, y, color);
                display.DrawLine(x + size, y + yOffset * 3 / 2, x + size - xOffset, y + yOffset * 2, color);
                display.DrawLine(x + xOffset, y + yOffset * 2, x + size - xOffset, y + yOffset * 2, color);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
        }
    }

    private static void DrawStar(int x, int y, int size, IDisplay display, Color color)
    {
        var halfSize = size / 2;
        var innerRadius = size / 4;

        var centerX = x + halfSize;
        var centerY = y + halfSize;

        // Draw 5 outer points
        var outerPoints = new PointF[5];
        var angle = -Math.PI / 2;
        for (int i = 0; i < 5; i++)
        {
            outerPoints[i] = new PointF((float)(centerX + halfSize * Math.Cos(angle)), (float)(centerY + halfSize * Math.Sin(angle)));
            angle += 2 * Math.PI / 5;
        }

        // Draw 5 inner points
        var innerPoints = new PointF[5];
        angle = -Math.PI / 2 + Math.PI / 5;
        for (var i = 0; i < 5; i++)
        {
            innerPoints[i] = new PointF((float)(centerX + innerRadius * Math.Cos(angle)), (float)(centerY + innerRadius * Math.Sin(angle)));
            angle += 2 * Math.PI / 5;
        }

        // Draw 10 lines connecting outer and inner points
        for (var i = 0; i < 5; i++)
        {
            display.DrawLine((int)outerPoints[i].X, (int)outerPoints[i].Y, (int)innerPoints[(i + 1) % 5].X, (int)innerPoints[(i + 1) % 5].Y, color);
        }
    }
}