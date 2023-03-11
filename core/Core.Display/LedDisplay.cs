using Core.Display.LedMatrix;

namespace Core.Display;

public class LedDisplay
{
    private RgbLedMatrix matrix;
    private RgbLedCanvas canvas;
    
    public LedDisplay()
    {
        matrix = new RgbLedMatrix(new RgbLedMatrixOptions
        {
            ChainLength = 1,
            Rows = 64,
            Cols = 64,
        });
        canvas = matrix.CreateOffscreenCanvas();
    }

    public void Clear()
    {
        canvas.Clear();
    }

    public void Update()
    {
        canvas = matrix.SwapOnVsync(canvas);
    }

    public void SetPixel(int x, int y, Color color)
    {
        canvas.SetPixel(x, y, color);
    }
}