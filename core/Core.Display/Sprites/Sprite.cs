using System.Drawing;

namespace Core.Display.Sprites;

public class Sprite : ISprite
{
    private Color?[,] map;
    public Sprite(Color?[,] map, int width, int height)
    {
        this.map = map;
        Width = width;
        Height = height;
    }
    
    public int Width { get; }
    public int Height { get; }

    public Color? GetColor(int frameNo, int x, int y) => map[x, y];
    
    public void Draw(IDisplay display, int x, int y, int frameNo = 0)
    {
        for (var sy = 0; sy < this.Height; sy++)
        {
            for (var sx = 0; sx < this.Width; sx++)
            {
                var color = this.GetColor(frameNo, sx, sy);
                if (color != null) display.SetPixel(x+sx, y+sy, color.Value);
            }
        }
    }
}