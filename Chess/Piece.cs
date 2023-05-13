namespace Chess;

public record Piece(PieceType Type, PieceColor Color, bool HasMoved = false);