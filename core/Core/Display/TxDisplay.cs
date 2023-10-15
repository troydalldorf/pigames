using System.Drawing;

namespace Core.Display;

public class TxDisplay: IDisplay, IDirectCanvasAccess
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

    public void Update(int? frameIntervalMs = null)
    {
        concrete.Update(frameIntervalMs);
    }

    public void SetPixel(int x, int y, Color color)
    {
        concrete.SetPixel(xTransform(x, y), yTransform(x, y), color);
    }

    public void DrawCircle(int x, int y, int radius, Color color, Color? fillColor = null)
    {
        concrete.DrawCircle(xTransform(x, y), yTransform(x, y), radius, color, fillColor);
    }

    public void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        concrete.DrawLine(xTransform(x0, y0), yTransform(x0, y0), xTransform(x1, y1), yTransform(x1, y1), color);
    }

    public void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null)
    {
        var tx1 = xTransform(x, y);
        var ty1 = yTransform(x, y);
        var tx2 = xTransform(x+width, y+height)-1;
        var ty2 = yTransform(x+width, y+height)-1;
        var twidth = tx2 - tx1+1;
        var theight = ty2 - ty1+1;
        concrete.DrawRectangle(tx1, ty1, twidth, theight, color, fillColor);
    }

    object IDirectCanvasAccess.GetCanvas()
    {
        return ((IDirectCanvasAccess)concrete).GetCanvas();
    }
}