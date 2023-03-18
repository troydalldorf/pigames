using System.Drawing;
using Core.Display;

namespace Core.Effects;

public class PixelBomb
{
    private List<Spark> sparks;

    public PixelBomb(double centerX, double centerY, int numPixels, Color color)
    {
        sparks = new List<Spark>();

        var rand = new Random();
        for (var i = 0; i < numPixels; i++)
        {
            var angle = rand.NextDouble() * 2 * Math.PI;
            var velocity = rand.NextDouble() * 5 + 5;

            var spark = new Spark();
            spark.X = centerX;
            spark.Y = centerY;
            spark.Color = color;
            spark.VelocityX = velocity * Math.Cos(angle);
            spark.VelocityY = velocity * Math.Sin(angle);

            sparks.Add(spark);
        }
    }

    public PixelBomb(Spark[] initialSparks)
    {
        sparks = new List<Spark>(initialSparks);
    }

    public bool IsExtinguished() => sparks.Count == 0;

    public void Update()
    {
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

    public void Draw(LedDisplay display)
    {
        foreach (var spark in sparks)
        {
            var x = (int)Math.Round(spark.X);
            var y = (int)Math.Round(spark.Y);
            display.SetPixel(x, y, spark.Color);
        }
    }
}