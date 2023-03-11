using System.Drawing;

namespace Core.Display.Sprites;

public class SpriteAnimation : ISprite
{
    private Sprite[] sprites;
    
    public SpriteAnimation(int width, int height, params Sprite[] sprites)
    {
        this.sprites = sprites;
        Frame = 0;
        Width = width;
        Height = height;
    }

    public Color? GetColor(int x, int y)
    {
        return sprites[Frame].GetColor(x, y);
    }
    
    public int Frame { get; set; }
    public int Width { get; }
    public int Height { get; }
}