using System.Drawing;
using Core.Display;

public class PixelBomb
{
    private class Pixel
    {
        public Point Position { get; set; }
        public Color Color { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        public int Life { get; set; }
    }

    private const int DefaultNumPixels = 30;
    private const int DefaultLifespan = 30;
    private const int DefaultRadius = 10;
    private const int DefaultGravity = 1;

    private readonly List<Pixel> pixels = new List<Pixel>();
    private readonly Random random = new Random();

    private int numPixels;
    private int lifespan;
    private int radius;
    private int gravity;

    public PixelBomb(int x, int y, int numPixels = DefaultNumPixels, int lifespan = DefaultLifespan, int radius = DefaultRadius, int gravity = DefaultGravity)
    {
        this.numPixels = numPixels;
        this.lifespan = lifespan;
        this.radius = radius;
        this.gravity = gravity;
        SpawnPixels(x, y);
    }

    public void Update()
    {
        for (var i = pixels.Count - 1; i >= 0; i--)
        {
            var pixel = pixels[i];
            pixel.Position = new Point(pixel.Position.X + pixel.VelocityX, pixel.Position.Y + pixel.VelocityY + gravity);
            pixel.Life--;
            if (pixel.Life <= 0)
            {
                pixels.RemoveAt(i);
            }
        }
    }

    public void Draw(LedDisplay display)
    {
        foreach (var pixel in pixels)
        {
            display.SetPixel(pixel.Position.X, pixel.Position.Y,  pixel.Color);
        }
    }

    public bool IsExtinguished => pixels.Count == 0;

    private void SpawnPixels(int centerX, int centerY)
    {
        for (var i = 0; i < numPixels; i++)
        {
            var angle = random.Next(360);
            var radians = angle * Math.PI / 180;
            var velocity = random.Next(3, 8);
            var color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            var x = (int)Math.Round(centerX + radius * Math.Cos(radians));
            var y = (int)Math.Round(centerY + radius * Math.Sin(radians));

            pixels.Add(new Pixel
            {
                Position = new Point(x, y),
                Color = color,
                VelocityX = (int)Math.Round(velocity * Math.Cos(radians)),
                VelocityY = (int)Math.Round(velocity * Math.Sin(radians)),
                Life = lifespan
            });
        }
    }
}