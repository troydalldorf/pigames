using System.Drawing;

namespace Core;

public interface IDisplay
{
    int Width { get; }
    int Height { get; }
    void Clear();
    void Update(int? frameIntervalMs);
    void SetPixel(int x, int y, Color color);
    void DrawCircle(int x, int y, int radius, Color color);
    void DrawLine(int x0, int y0, int x1, int y1, Color color);
    void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null);
}