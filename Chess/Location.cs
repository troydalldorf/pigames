namespace Chess;

public record Location(int X, int Y)
{
    public Location MoveLeft() => this with { X = Math.Clamp(X - 1, 0, 7) };
    public Location MoveRight() => this with { X = Math.Clamp(X + 1, 0, 7) };
    public Location MoveUp() => this with { Y = Math.Clamp(Y - 1, 0, 7) };
    public Location MoveDown() => this with { Y = Math.Clamp(Y + 1, 0, 7) };
}
