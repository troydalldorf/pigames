using System.Diagnostics;
using System.Drawing;
using Core.Display.LedMatrix;

namespace Core.Display;

public class LedDisplay
{
    private RgbLedMatrix matrix;
    private RgbLedCanvas canvas;
    private Stopwatch stopwatch;
    
    public LedDisplay()
    {
        matrix = new RgbLedMatrix(new RgbLedMatrixOptions
        {
            ChainLength = 1,
            Rows = 64,
            Cols = 64,
        });
        canvas = matrix.CreateOffscreenCanvas();
        stopwatch = new Stopwatch();
    }

    public int Width => 64;
    public int Height => 64;

    public void Clear()
    {
        canvas.Clear();
    }

    public void Update()
    {
        // force 30 FPS
        var elapsed= stopwatch.ElapsedMilliseconds;
        if (elapsed < 33)
        {
            Thread.Sleep(33 - (int)elapsed);
        }
        canvas = matrix.SwapOnVsync(canvas);
        stopwatch.Restart();
    }

    public void SetPixel(int x, int y, Color color)
    {
        canvas.SetPixel(x, y, color);
    }
    
    public void DrawCircle(int x, int y, int radius, Color color)
    {
        canvas.DrawCircle(x, y, radius, color);
    }

    public void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        canvas.DrawLine(x0, y0, x1, y1, color);
    }
    
    public void DrawRectangle(int x, int y, int width, int height, Color color)
    {
        var x2 = x + width - 1;
        var y2 = y + height - 1;
        canvas.DrawLine(x, y, x2, y, color);
        canvas.DrawLine(x2, y, x2, y2, color);
        canvas.DrawLine(x2, y2, x, y2, color);
        canvas.DrawLine(x, y2, x, y, color);
    }

    internal RgbLedCanvas GetCanvas() => canvas;
}