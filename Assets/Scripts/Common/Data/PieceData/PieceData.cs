using System;

[Serializable]
public class PieceData
{
    public PieceType Type = PieceType.None;
    public PieceColor Color = PieceColor.None;

    /// <summary>
    /// Create empty PieceData
    /// </summary>
    public PieceData()
    {
        Type = PieceType.None;
        Color = PieceColor.None;
    }
    /// <summary>
    /// Create concrete PieceData
    /// </summary>
    /// <param name="type"></param>
    /// <param name="color"></param>
    public PieceData(PieceType type, PieceColor color)
    {
        Type = type;
        Color = color;
    }

    public PieceData Clone() => new(Type, Color);
}

[Serializable]
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
[Serializable]
public enum PieceColor
{
    None = -1,
    White,
    Black,
    Other = 100
}