using Core.Display.Sprites;

namespace Chess;

public class ChessBoard
{
    private readonly Piece?[,] board = new Piece[8, 8];
    private Move? lastMove = null;

    public void Reset()
    {
        board[0, 0] = new Piece(PieceType.Rook, PieceColor.Black);
        board[1, 0] = new Piece(PieceType.Knight, PieceColor.Black);
        board[2, 0] = new Piece(PieceType.Bishop, PieceColor.Black);
        board[3, 0] = new Piece(PieceType.Queen, PieceColor.Black);
        board[4, 0] = new Piece(PieceType.King, PieceColor.Black);
        board[5, 0] = new Piece(PieceType.Bishop, PieceColor.Black);
        board[6, 0] = new Piece(PieceType.Knight, PieceColor.Black);
        board[7, 0] = new Piece(PieceType.Rook, PieceColor.Black);
        for (int i = 0; i < 8; i++)
        {
            board[i, 1] = new Piece(PieceType.Pawn, PieceColor.Black);
        }

        board[0, 7] = new Piece(PieceType.Rook, PieceColor.White);
        board[1, 7] = new Piece(PieceType.Knight, PieceColor.White);
        board[2, 7] = new Piece(PieceType.Bishop, PieceColor.White);
        board[3, 7] = new Piece(PieceType.Queen, PieceColor.White);
        board[4, 7] = new Piece(PieceType.King, PieceColor.White);
        board[5, 7] = new Piece(PieceType.Bishop, PieceColor.White);
        board[6, 7] = new Piece(PieceType.Knight, PieceColor.White);
        board[7, 7] = new Piece(PieceType.Rook, PieceColor.White);
        for (var i = 0; i < 8; i++)
        {
            board[i, 6] = new Piece(PieceType.Pawn, PieceColor.White);
        }
    }

    public Piece? GetPieceAt(Location location)
    {
        return GetPieceAt(location.X, location.Y);
    }

    public Piece? GetPieceAt(int x, int y)
    {
        if (x is < 0 or > 7 || y is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException($"Location ({x}, {y}) is out of bounds.");
        }
        return board[x, y];
    }

    public bool TryMove(Location? from, Location? to)
    {
        if (from == null || to == null)
        {
            return false;
        }

        if (!IsValidMove(from, to)) return false;

        Move(from, to);
        return true;
    }

    private void Move(Location from, Location to)
    {
        lastMove = new Move(from, to, board[to.X, to.Y]!);
        board[to.X, to.Y] = board[from.X, from.Y]! with { HasMoved = true };
        board[from.X, from.Y] = null;
    }

    private void UndoLast()
    {
        if (lastMove == null)
        {
            return;
        }

        board[lastMove.From.X, lastMove.From.Y] = board[lastMove.To.X, lastMove.To.Y]!;
        board[lastMove.To.X, lastMove.To.Y] = lastMove.CapturedPiece;
    }

    private bool IsValidMove(Location from, Location to)
    {
        if (!IsValidBasicMove(from, to)) return false;
        var currentPiece = GetPieceAt(from);
        Move(from, to);
        var isInCheck = IsCheck(currentPiece!.Color);
        UndoLast();
        return !isInCheck;
    }

    private bool IsValidBasicMove(Location from, Location to)
    {
        var currentPiece = board[from.X, from.Y];
        var targetPiece = board[to.X, to.Y];

        if (currentPiece == null)
        {
            return false;
        }

        if (targetPiece != null && targetPiece.Color == currentPiece.Color)
        {
            return false;
        }

        var deltaX = Math.Abs(to.X - from.X);
        var deltaY = Math.Abs(to.Y - from.Y);

        switch (currentPiece.Type)
        {
            case PieceType.Pawn:
                // Pawns can only move forward one space (two on their first move), 
                // and can only capture diagonally
                if (targetPiece == null)
                {
                    if (currentPiece.Color == PieceColor.White)
                    {
                        if (deltaX == 0 && (deltaY == 1 || (!currentPiece.HasMoved && deltaY == 2)) && to.Y < from.Y)
                        {
                            return IsPathClear(from, to, false);
                        }
                    }
                    else
                    {
                        if (deltaX == 0 && (deltaY == 1 || (!currentPiece.HasMoved && deltaY == 2)) && to.Y > from.Y)
                        {
                            return IsPathClear(from, to, false);
                        }
                    }
                }
                else
                {
                    if (deltaX == 1 && deltaY == 1)
                    {
                        return true;
                    }
                }

                break;
            case PieceType.Rook:
                // Rooks can move any number of spaces along any row or column
                if (deltaX * deltaY == 0)
                {
                    return IsPathClear(from, to, true);
                }

                break;
            case PieceType.Knight:
                // Knights move in an L shape: two spaces along a row or column, and then one space perpendicular
                if (deltaX * deltaY == 2)
                {
                    return true;
                }

                break;
            case PieceType.Bishop:
                // Bishops move any number of spaces diagonally
                if (deltaX == deltaY)
                {
                    return IsPathClear(from, to, true);
                }

                break;
            case PieceType.Queen:
                // Queens can move any number of spaces along any row, column, or diagonal
                if (deltaX * deltaY == 0 || deltaX == deltaY)
                {
                    return IsPathClear(from, to, true);
                }

                break;
            case PieceType.King:
                // Kings can move one space in any direction
                if (deltaX <= 1 && deltaY <= 1)
                {
                    return IsPathClear(from, to, true);
                }

                break;
        }

        return false;
    }

    private bool IsPathClear(Location from, Location to, bool dontCheckEnd)
    {
        // determine direction of movement
        var xDir = from.X < to.X ? 1 : from.X > to.X ? -1 : 0;
        var yDir = from.Y < to.Y ? 1 : from.Y > to.Y ? -1 : 0;
        var x = from.X;
        var y = from.Y;
        
        if (dontCheckEnd) to = new Location(to.X - xDir, to.Y - yDir);
        if (to == from) return true;
        
        do
        {
            x += xDir;
            y += yDir;
            if (GetPieceAt(x, y) != null)
            {
                return false;
            }
        } while (!(x == to.X && y == to.Y));

        // path is clear
        return true;
    }

    private IEnumerable<Move> GetAllPossibleMoves(PieceColor color)
    {
        var moves = new List<Move>();

        for (var x = 0; x < 8; x++)
        {
            for (var y = 0; y < 8; y++)
            {
                var piece = GetPieceAt(x, y);
                if (piece?.Color != color) continue;
                var from = new Location(x, y);
                for (var x2 = 0; x2 < 8; x2++)
                {
                    for (var y2 = 0; y2 < 8; y2++)
                    {
                        var to = new Location(x2, y2);
                        if (IsValidBasicMove(from, to))
                        {
                            moves.Add(new Move(from, to));
                        }
                    }
                }
            }
        }

        return moves;
    }

    public bool IsCheck(PieceColor color)
    {
        // Find the king
        Location? kingLocation = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = GetPieceAt(i, j);
                if (piece?.Type == PieceType.King && piece.Color == color)
                {
                    kingLocation = new Location(i, j);
                    break;
                }
            }
        }

        if (kingLocation == null)
        {
            throw new Exception("King not found");
        }

        // Check if any of the opponent's moves can capture the king
        var opponentColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        var opponentMoves = GetAllPossibleMoves(opponentColor).Select(x => x.To);
        return opponentMoves.Contains(kingLocation);
    }

    public bool IsCheckmate(PieceColor color)
    {
        if (!IsCheck(color))
        {
            return false;
        }

        return !GetAllPossibleMoves(color).Any();
    }
}