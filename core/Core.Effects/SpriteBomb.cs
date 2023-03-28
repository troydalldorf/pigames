using Core.Display.Sprites;

namespace Core.Effects;

public class SpriteBomb : PixelBomb
{
    public SpriteBomb(int x, int y, ISprite sprite, int frameNo = 0, int tail = 0) : base(SparksFromSprite(x, y, sprite, frameNo, tail))
    {
    }

    private static Spark[] SparksFromSprite(int x, int y, ISprite sprite, int frameNo = 0, int tail = 0)
    {
        var sparks = new List<Spark>();
        for (var sy = 0; sy < sprite.Height; sy++)
        {
            for (var sx = 0; sx < sprite.Width; sx++)
            {
                var color = sprite.GetColor(frameNo, sx, sy);
                if (color != null) sparks.Add(new Spark(x + sx, y + sy, color.Value, tail));
            }
        }

        return sparks.ToArray();
    }
}