public static class PiecePacker
{
    public static void PackPiece(PieceType pieceType, PieceColor pieceColor, out byte packedPiece) => packedPiece = (byte)((byte)pieceType | ((byte)pieceColor << 3));
    public static byte PackPiece(PieceType pieceType, PieceColor pieceColor) => (byte)((byte)pieceType | ((byte)pieceColor << 3));

    public static void GetPieceType(ref byte packedPiece, out PieceType pieceType) => pieceType = (PieceType)(packedPiece & 0x07);
    public static PieceType GetPieceType(byte packedPiece) => (PieceType)(packedPiece & 0x07);

    public static void GetPieceColor(ref byte packedPiece, out PieceColor pieceColor) => pieceColor = (PieceColor)((packedPiece >> 3) & 0x01);
    public static PieceColor GetPieceColor(byte packedPiece) => (PieceColor)((packedPiece >> 3) & 0x01);

    // Жесткая упаковка двух распакованных фигур
    public static void HardPackPiece(PieceType pieceType1, PieceColor pieceColor1, PieceType pieceType2, PieceColor pieceColor2, out byte packedPieces)
    {
        PackPiece(pieceType1, pieceColor1, out byte piece1);
        PackPiece(pieceType2, pieceColor2, out byte piece2);
        packedPieces = (byte)(piece1 | (piece2 << 4));
    }

    //Жёсткая упаковка второй фигуры в байт
    public static void HardPackPiece(ref byte piece1, PieceType pieceType2, PieceColor pieceColor2, out byte packedPieces)
    {
        PackPiece(pieceType2, pieceColor2, out byte piece2);
        packedPieces = (byte)(piece1 | (piece2 << 4));
    }

    public static byte GetHardPieceType(ref byte packedPiece, bool isFirst)
    {
        if (isFirst)
            return (byte)(packedPiece & 0x07);
        else
            return (byte)((packedPiece >> 4) & 0x07);
    }

    public static byte GetHardPieceColor(ref byte packedPiece, bool isFirst)
    {
        if (isFirst)
            return (byte)((packedPiece >> 3) & 0x01);
        else
            return (byte)((packedPiece >> 7) & 0x01);
    }
}