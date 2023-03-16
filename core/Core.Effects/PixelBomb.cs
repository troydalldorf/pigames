using Core.Display;

namespace Core.Effects;

using System;
using System.Collections.Generic;
using System.Drawing;

public class PixelBomb
{
    private Random random = new();
    private readonly List<Spark> sparks = new();
    private readonly int fadeFrames;
    private readonly int fallSpeed;
    private int elapsedFrames = 0;

    public bool Complete => elapsedFrames > fadeFrames;

    public PixelBomb(int x, int y, int size, int numPixels, int durationFrames, int fadeFrames, int fallSpeed)
    {
        this.fadeFrames = fadeFrames;
        this.fallSpeed = fallSpeed;

        var speed = random.NextDouble() * size;
        for (var i = 0; i < numPixels; i++)
        {
            var angle = random.NextDouble() * 2 * Math.PI;
            var lifetime = random.Next(durationFrames / 2, durationFrames);
            var spark = new Spark(x, y, angle, speed, lifetime);
            spark.Color = Color.FromArgb(255, random.Next(256), random.Next(256), random.Next(256));
            sparks.Add(spark);
        }
    }

    public void Update()
    {
        if (Complete) return;

        foreach (var spark in sparks)
        {
            spark.Lifetime--;

            if (elapsedFrames < fadeFrames)
            {
                // Fade out the pixel by reducing its brightness
                var brightness = 255 * (1 - (float)elapsedFrames / fadeFrames);
                spark.Color = Color.FromArgb(255, (int)brightness, (int)brightness, (int)brightness);
            }

            if (spark.Lifetime <= 0 && elapsedFrames >= fadeFrames)
            {
                spark.Y += fallSpeed;
            }
        }

        elapsedFrames++;
    }

    public void Draw(LedDisplay display)
    {
        for (var i = 0; i < sparks.Count; i++)
        {
            var spark = sparks[i];
            if (spark.Lifetime <= 0 && elapsedFrames >= fadeFrames) continue;

            var newX = (int)(spark.X + Math.Cos(spark.Angle) * spark.Speed);
            var newY = (int)(spark.Y + Math.Sin(spark.Angle) * spark.Speed);
            display.SetPixel(newX, newY, spark.Color);
        }
    }
}