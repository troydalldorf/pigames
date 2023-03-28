using System.Drawing;

namespace Core.Effects;

public class Spark : IGameElement
{
    private readonly int tail;
    private Queue<Tuple<double, double>> tailPoints;
    private static readonly Random Random = new Random();
    
    public Spark(int x, int y, Color color, int tail = 0)
    {
        this.tail = tail;
        var angle = Random.NextDouble() * 2 * Math.PI;
        var velocity = Random.NextDouble() * 5 + 5;
        this.X = x;
        this.Y = y;
        this.Color = color;
        this.VelocityX = velocity * Math.Cos(angle);
        this.VelocityY = velocity * Math.Sin(angle);
        if (tail > 0) tailPoints = new Queue<Tuple<double, double>>(tail);
    }
    
    public double X { get; set; }
    public double Y { get; set; }
    public Color Color { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    
    public void Update()
    {
        if (tail > 0) tailPoints?.Enqueue(new Tuple<double, double>(X, Y));
        X += VelocityX;
        Y += VelocityY;
        VelocityX *= 0.98;
        VelocityY *= 0.98;
        if (tail > 0 && tailPoints?.Count > tail) tailPoints.Dequeue();
    }

    public void Draw(IDisplay display)
    {
        var x = (int)Math.Round(X);
        var y = (int)Math.Round(Y);
        display.SetPixel(x, y, Color);
        if (tail > 0)
        {
            foreach (var point in tailPoints)
                display.SetPixel((int)Math.Round(point.Item1), (int)Math.Round(point.Item2), this.Color.Fade(1d/tail));
        }
    }
}