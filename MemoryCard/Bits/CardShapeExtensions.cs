using System.Drawing;
using Core;

public static class CardShapeExtensions
{
    public static void Draw(this CardShape shape, IDisplay display, int x, int y, int size)
    {
        var color = Color.FromArgb(255, 255, 255);

        switch (shape)
        {
            case CardShape.Circle:
                display.DrawCircle(x + size / 2, y + size / 2, size / 2, color);
                break;
            case CardShape.Triangle:
                display.DrawLine(x, y + size, x + size / 2, y, color);
                display.DrawLine(x + size / 2, y, x + size, y + size, color);
                display.DrawLine(x + size, y + size, x, y + size, color);
                break;
            case CardShape.Square:
                display.DrawRectangle(x, y, size, size, color, color);
                break;
            case CardShape.Diamond:
                display.DrawLine(x + size / 2, y, x, y + size / 2, color);
                display.DrawLine(x, y + size / 2, x + size / 2, y + size, color);
                display.DrawLine(x + size / 2, y + size, x + size, y + size / 2, color);
                display.DrawLine(x + size, y + size / 2, x + size / 2, y, color);
                break;
            case CardShape.Star:
                // Add star drawing logic here.
                break;
            case CardShape.Plus:
                var lineWidth = size / 4;
                display.DrawRectangle(x + lineWidth, y, lineWidth, size, color, color);
                display.DrawRectangle(x, y + lineWidth, size, lineWidth, color, color);
                break;
            case CardShape.Cross:
                display.DrawLine(x, y, x + size, y + size, color);
                display.DrawLine(x + size, y, x, y + size, color);
                break;
            case CardShape.Hexagon:
                var xOffset = size / 4;
                var yOffset = size / 2;
                display.DrawLine(x + xOffset, y, x + size - xOffset, y, color);
                display.DrawLine(x, y + yOffset / 2, x + xOffset, y, color);
                display.DrawLine(x, y + yOffset * 3 / 2, x + xOffset, y + yOffset * 2, color);
                display.DrawLine(x + size, y + yOffset / 2, x + size - xOffset, y, color);
                display.DrawLine(x + size, y + yOffset * 3 / 2, x + size - xOffset, y + yOffset * 2, color);
                display.DrawLine(x + xOffset, y + yOffset * 2, x + size - xOffset, y + yOffset * 2, color);
                break;
        }
    }
}