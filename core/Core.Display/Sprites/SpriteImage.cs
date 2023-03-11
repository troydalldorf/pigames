using System.Drawing;

namespace Core.Display.Sprites;

public class SpriteImage
{
    private readonly Bitmap image;
    
    public SpriteImage(string filePath)
    {
        image = new Bitmap(filePath);
    }

    public Sprite GetSprite(int x, int y, int width, int height)
    {
        return GetSprite(new Rectangle(x, y, width, height));
    }

    public Sprite GetSprite(Rectangle from)
    {
        var spriteBitmap = new Bitmap(from.Width, from.Height);
        using (var g = Graphics.FromImage(spriteBitmap))
            g.DrawImage(image, 0, 0, from, GraphicsUnit.Pixel);
        var colors = new Color[spriteBitmap.Width, spriteBitmap.Height];
        for (var x = 0; x <= spriteBitmap.Width; x++)
            for (var y = 0; y <= spriteBitmap.Height; y++)
                colors[x, y] = spriteBitmap.GetPixel(x, y);
        return new Sprite(colors, spriteBitmap.Width, spriteBitmap.Height);
    }
}