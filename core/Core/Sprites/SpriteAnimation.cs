using System.Drawing;

namespace Core.Display.Sprites;

public class SpriteAnimation : ISprite
{
    private Sprite[] sprites;
    
    public SpriteAnimation(int width, int height, params Sprite[] sprites)
    {
        this.sprites = sprites;
        Width = width;
        Height = height;
    }

    public Color? GetColor(int frameNo, int x, int y)
    {
        return sprites[frameNo].GetColor(0, x, y);
    }

    public ISprite GetFrame(int frameNo)
    {
        return sprites[frameNo];
    }
    
    public int Width { get; }
    
    public int Height { get; }
    
    public void Draw(IDisplay display, int x, int y, int frameNo = 0)
    {
        GetFrame(frameNo).Draw(display, x, y, frameNo);
    }
}