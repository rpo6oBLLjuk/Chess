public static class PieceColorExtensions
{
    public static PieceColor Invert(this PieceColor color)
    {
        return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }
}