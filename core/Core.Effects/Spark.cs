namespace Core.Effects;

public class Spark
{
    public int X { get; set; }
    public int Y { get; set; }
    public double Angle { get; set; }
    public double Speed { get; set; }
    public int Lifetime { get; set; }

    public Spark(int x, int y, double angle, double speed, int lifetime)
    {
        X = x;
        Y = y;
        Angle = angle;
        Speed = speed;
        Lifetime = lifetime;
    }
}