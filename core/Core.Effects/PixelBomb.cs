using System.Drawing;
using Core.Display;

namespace Core.Effects;

public class PixelBomb
{
    private List<Spark> sparks;
    private int updateCount;

    public PixelBomb(int centerX, int centerY, int numPixels, Color color)
    {
        sparks = new List<Spark>();
        for (var i = 0; i < numPixels; i++)
        {
            sparks.Add(new Spark(centerX, centerY, color));
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
            spark.X += spark.VelocityX;
            spark.Y += spark.VelocityY;
            spark.VelocityX *= 0.98;
            spark.VelocityY *= 0.98;
            if (spark.X < 0 | spark.X > 63 | spark.Y < 0 | spark.Y > 63)
                sparks.Remove(spark);
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var spark in sparks)
        {
            var x = (int)Math.Round(spark.X);
            var y = (int)Math.Round(spark.Y);
            display.SetPixel(x, y, spark.Color);
        }
    }
}