using System.Drawing;
using System.Drawing.Imaging;

namespace Core.Display.Sprites;

public class SpriteImage
{
    private readonly Bitmap image;
    private readonly Color? transparent;
    
    public SpriteImage(string filePath, Point? transparentRef)
    {
        image = new Bitmap(filePath);
        if (transparentRef != null)
        {
            transparent = image.GetPixel(transparentRef.Value.X, transparentRef.Value.Y);
        }
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
        var colors = new Color?[spriteBitmap.Width, spriteBitmap.Height];
        for (var x = 0; x < spriteBitmap.Width; x++)
        for (var y = 0; y < spriteBitmap.Height; y++)
        {
            var color = spriteBitmap.GetPixel(x, y);
            colors[x, y] = transparent == null | color != transparent  ? color : null;
        }
        return new Sprite(colors, spriteBitmap.Width, spriteBitmap.Height);
    }

    public SpriteAnimation GetSpriteAnimation(int x, int y, int width, int height, int count, int xspace)
    {
        var sprites = new List<Sprite>();
        for (var i = 0; i < count; i++)
        {
            sprites.Add(this.GetSprite(x, y, width, height));
            x += width + xspace;
        }

        return new SpriteAnimation(width, height, sprites.ToArray());
    }
}