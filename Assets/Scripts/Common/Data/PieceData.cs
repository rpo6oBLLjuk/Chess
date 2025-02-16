public class PieceData
{
    public PieceType Type { get; set; }
    public PieceColor Color { get; set; }

    public PieceData(PieceType type, PieceColor color)
    {
        Type = type;
        Color = color;
    }

    public PieceData Clone() => new(Type, Color);
}

public enum PieceType
{
    None = -1,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
    Other = 100
}

public enum PieceColor
{
    None = -1,
    White,
    Black,
    Other = 100
}