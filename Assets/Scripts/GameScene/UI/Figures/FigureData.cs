public class FigureData
{
    public FigureType Type { get; set; }
    public FigureColor Color { get; set; }
}

public enum FigureType
{
    None,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}

public enum FigureColor
{
    None,
    White,
    Black
}