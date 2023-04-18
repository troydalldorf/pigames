using System.Drawing;
using Core.Display.Sprites;

namespace Core.Sprites;

public static class SpriteExtensions
{
    public static void DrawRotated(this ISprite @this, IDisplay display, int x, int y, double angle, int frameNo = 0)
    {
        var halfWidth = @this.Width / 2;
        var halfHeight = @this.Height / 2;

        var radianAngle = Math.PI * angle / 180.0;
        var cosAngle = Math.Cos(radianAngle);
        var sinAngle = Math.Sin(radianAngle);

        for (var sy = -halfHeight; sy < halfHeight; sy++)
        {
            for (var sx = -halfWidth; sx < halfWidth; sx++)
            {
                var rotatedX = (int)Math.Round(cosAngle * sx - sinAngle * sy) + x;
                var rotatedY = (int)Math.Round(sinAngle * sx + cosAngle * sy) + y;

                var sourceX = sx + halfWidth;
                var sourceY = sy + halfHeight;

                if (sourceX >= 0 && sourceX < @this.Width && sourceY >= 0 && sourceY < @this.Height)
                {
                    var color = @this.GetColor(frameNo, sourceX, sourceY);
                    if (color != null) display.SetPixel(rotatedX, rotatedY, color.Value);
                }
            }
        }
    }
    
    public static void DrawTiled(this ISprite @this, IDisplay display, Rectangle targetRectangle, int frameNo = 0)
    {
        for (var y = 0; y < targetRectangle.Height; y++)
        {
            for (var x = 0; x < targetRectangle.Width; x++)
            {
                var sx = x % @this.Width;
                var sy = y % @this.Height;
                var color = @this.GetColor(frameNo, sx, sy);
                if (color != null) display.SetPixel(targetRectangle.X + x, targetRectangle.Y + y, color.Value);
            }
        }
    }
}