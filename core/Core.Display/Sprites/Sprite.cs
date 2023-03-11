using System.Drawing;

namespace Core.Display.Sprites;

public class Sprite
{
    private Color[,] map;
    public Sprite(Color[,] map, int width, int height)
    {
        this.map = map;
        Width = width;
        Height = height;
    }
    
    public int Width { get; }
    public int Height { get; }

    public Color GetColor(int x, int y) => map[x, y];
}