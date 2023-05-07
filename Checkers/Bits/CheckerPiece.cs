namespace Checkers.Bits;

public class CheckerPiece
{
    public bool IsPlayer1 { get; }
    public bool IsKing { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool HasCaptured { get; set; }

    public CheckerPiece(bool isPlayer1, bool isKing, int x, int y)
    {
        IsPlayer1 = isPlayer1;
        IsKing = isKing;
        X = x;
        Y = y;
        HasCaptured = false;
    }
}