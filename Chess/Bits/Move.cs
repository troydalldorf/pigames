namespace Chess;

public record Move(Location From, Location To, Piece? CapturedPiece = null);