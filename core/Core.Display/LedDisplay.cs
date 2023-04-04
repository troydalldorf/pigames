using System.Diagnostics;
using System.Drawing;
using Core.Display.LedMatrix;

namespace Core.Display;

public class LedDisplay : IDisplay, IDirectCanvasAccess
{
    private readonly RgbLedMatrix matrix;
    private RgbLedCanvas canvas;
    private readonly Stopwatch stopwatch;
    
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

    public void Update(int? frameIntervalMs = null)
    {
        frameIntervalMs ??= 33;
        var elapsed= stopwatch.ElapsedMilliseconds;
        if (elapsed < frameIntervalMs)
        {
            Thread.Sleep(frameIntervalMs.Value - (int)elapsed);
        }
        canvas = matrix.SwapOnVsync(canvas);
        stopwatch.Restart();
    }

    public void SetPixel(int x, int y, Color color)
    {
        canvas.SetPixel(x, y, color);
    }
    
    public void DrawCircle(int x, int y, int radius, Color color, Color? fillColor = null)
    {
        if (fillColor != null)
        {
            for (var r = radius - 1; r > 0; r--)
            {
                canvas.DrawCircle(x, y, r, fillColor.Value);
            }
        }

        canvas.DrawCircle(x, y, radius, color);
    }

    public void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        canvas.DrawLine(x0, y0, x1, y1, color);
    }
    
    public void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null)
    {
        var x2 = x + width - 1;
        var y2 = y + height - 1;
        if (fillColor != null)
        {
            for (var row = y + 1; row< y2; row++)
                canvas.DrawLine(x+1, row, x2-1, row, fillColor.Value);
        }
        canvas.DrawLine(x, y, x2, y, color);
        canvas.DrawLine(x2, y, x2, y2, color);
        canvas.DrawLine(x2, y2, x, y2, color);
        canvas.DrawLine(x, y2, x, y, color);
    }

    object IDirectCanvasAccess.GetCanvas() => canvas;
}