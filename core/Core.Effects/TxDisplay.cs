using System.Drawing;

namespace Core.Effects;

public class TxDisplay: IDisplay
{
    private readonly IDisplay concrete;
    private readonly Func<int, int, int> xTransform;
    private readonly Func<int, int, int> yTransform;
    public int Width => concrete.Width;
    public int Height => concrete.Height;

    public TxDisplay(IDisplay concrete, Func<int, int, int> xTransform, Func<int, int, int> yTransform)
    {
        this.concrete = concrete;
        this.xTransform = xTransform;
        this.yTransform = yTransform;
    }
    
    public void Clear()
    {
        concrete.Clear();
    }

    public void Update()
    {
        concrete.Update();
    }

    public void SetPixel(int x, int y, Color color)
    {
        concrete.SetPixel(xTransform(x, y), yTransform(x, y), color);
    }

    public void DrawCircle(int x, int y, int radius, Color color)
    {
        concrete.DrawCircle(xTransform(x, y), yTransform(x, y), radius, color);
    }

    public void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        concrete.DrawLine(xTransform(x0, y0), yTransform(x0, y0), xTransform(x1, y1), yTransform(x1, y1), color);
    }

    public void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null)
    {
        concrete.DrawLine(xTransform(x, y), yTransform(x, y), width, height, color);
    }
}