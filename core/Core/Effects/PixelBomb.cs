using System.Drawing;
using Core.Display;

namespace Core.Effects;

public class PixelBomb
{
    private readonly List<Spark> sparks;
    private int updateCount;

    public PixelBomb(int centerX, int centerY, int numPixels, Color color, int tail = 0)
    {
        sparks = new List<Spark>();
        for (var i = 0; i < numPixels; i++)
        {
            sparks.Add(new Spark(centerX, centerY, color, tail));
        }
    }

    public PixelBomb(Spark[] initialSparks)
    {
        sparks = new List<Spark>(initialSparks);
    }

    public bool IsExtinguished() => sparks.Count == 0 || updateCount > 15;

    public void Update()
    {
        updateCount++;
        foreach (var spark in sparks.ToArray())
        {
            spark.Update();
            if (spark.X < 0 | spark.X > 63 | spark.Y < 0 | spark.Y > 63)
                sparks.Remove(spark);
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var spark in sparks)
        {
            spark.Draw(display);
        }
    }
}