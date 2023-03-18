using System.Drawing;

namespace Core.Effects;

public class Spark
{
    private static readonly Random Random = new Random();
    
    public Spark(int x, int y, Color color)
    {
        var angle = Random.NextDouble() * 2 * Math.PI;
        var velocity = Random.NextDouble() * 5 + 5;
        this.X = x;
        this.Y = y;
        this.Color = color;
        this.VelocityX = velocity * Math.Cos(angle);
        this.VelocityY = velocity * Math.Sin(angle);
    }
    
    public double X { get; set; }
    public double Y { get; set; }
    public Color Color { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
}