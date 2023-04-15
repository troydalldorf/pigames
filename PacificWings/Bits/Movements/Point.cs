namespace PacificWings.Bits.Movements;

public record Point(float X, float Y)
{
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}