using System.Drawing;

namespace Core.Display.Sprites;

public class SpriteAnimation : ISprite
{
    private Sprite[] sprites;
    
    public SpriteAnimation(int width, int height, params Sprite[] sprites)
    {
        this.sprites = sprites;
        FrameNo = 0;
        Width = width;
        Height = height;
    }

    public Color? GetColor(int x, int y)
    {
        return sprites[FrameNo].GetColor(x, y);
    }

    public ISprite GetFrame(int frameNo)
    {
        return sprites[frameNo];
    }
    
    public int FrameNo { get; set; }
    public int Width { get; }
    public int Height { get; }
}