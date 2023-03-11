using System.Diagnostics;
using System.Drawing;
using Core.Display.LedMatrix;
using Core.Display.Sprites;

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

    public void DrawSprite(int x, int y, ISprite sprite)
    {
        for (var sy = 0; sy < sprite.Height; sy++)
        {
            for (var sx = 0; sx < sprite.Width; sx++)
            {
                var color = sprite.GetColor(sx, sy);
                if (color != null) canvas.SetPixel(x+sx, y+sy, color.Value);
            }
        }
    }
}