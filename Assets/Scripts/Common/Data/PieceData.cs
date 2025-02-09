public class PieceData
{
    public PieceType Type { get; set; }
    public PieceColor Color { get; set; }
}

public enum PieceType
{
    None = -1,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}

public enum PieceColor
{
    None = -1,
    White,
    Black
}