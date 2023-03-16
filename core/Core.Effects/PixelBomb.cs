using Core.Display;

namespace Core.Effects;

using System;
using System.Collections.Generic;
using System.Drawing;

public class PixelBomb
{
    private Random random = new();
    private readonly List<Spark> sparks = new();
    public bool Complete { get; private set; }

    public PixelBomb(int x, int y, int size, int numPixels, int durationFrames)
    {

        var speed = random.NextDouble() * size;
        for (var i = 0; i < numPixels; i++)
        {
            var angle = random.NextDouble() * 2 * Math.PI;
            var lifetime = random.Next(durationFrames/2, durationFrames);
            sparks.Add(new Spark(x, y, angle, speed, lifetime));
        }
    }

    public void Update()
    {
        if (Complete) return;
        foreach (var spark in sparks)
            spark.Lifetime--;
        if (sparks.All(x => x.Lifetime > 0)) Complete = true;
    }

    public void Draw(LedDisplay display)
    {
        for (var i = 0; i < sparks.Count; i++)
        {
            var spark = sparks[i];
            if (spark.Lifetime <= 0) continue;
            var newX = (int)(spark.X + Math.Cos(spark.Angle) * spark.Speed);
            var newY = (int)(spark.Y + Math.Sin(spark.Angle) * spark.Speed);
            display.SetPixel(newX, newY, Color.FromArgb(255, random.Next(256), random.Next(256)));
        }
    }
}