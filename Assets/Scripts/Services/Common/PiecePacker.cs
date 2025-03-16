using System;

public static class PiecePacker
{
    public static void PackPiece(PieceType pieceType, PieceColor pieceColor, out byte packedPiece)
        => packedPiece = (byte)((byte)pieceType | (byte)pieceColor);

    public static byte PackPiece(PieceType pieceType, PieceColor pieceColor)
        => (byte)((byte)pieceType | (byte)pieceColor);

    public static void PackPiece(ref PieceType pieceType, ref PieceColor pieceColor, out byte packedPiece)
        => packedPiece = (byte)((byte)pieceType | (byte)pieceColor);

    public static byte PackPiece(ref PieceType pieceType, ref PieceColor pieceColor)
        => (byte)((byte)pieceType | (byte)pieceColor);

    public static void GetType(ref byte packedPiece, out PieceType pieceType)
        => pieceType = (PieceType)(packedPiece & 0x07); // Mask 0x07 (bits 0-2)

    public static PieceType GetType(ref byte packedPiece)
        => (PieceType)(packedPiece & 0x07);

    public static void GetColor(ref byte packedPiece, out PieceColor pieceColor)
        => pieceColor = (PieceColor)(packedPiece & 0x08); // Mask 0x08 (bit 3)

    public static PieceColor GetColor(ref byte packedPiece)
        => (PieceColor)(packedPiece & 0x08);

    #region HardPack
    // Жесткая упаковка двух распакованных фигур
    public static void HardPackPiece(ref PieceType pieceType1, ref PieceColor pieceColor1, ref PieceType pieceType2, ref PieceColor pieceColor2, out byte packedPieces)
    {
        PackPiece(ref pieceType1, ref pieceColor1, out byte piece1);
        PackPiece(ref pieceType2, ref pieceColor2, out byte piece2);
        packedPieces = (byte)(piece1 | (piece2 << 4));
    }
    //Жёсткая упаковка второй фигуры в байт
    public static void HardPackPiece(ref byte piece1, PieceType pieceType2, PieceColor pieceColor2, out byte packedPieces)
    {
        PackPiece(ref pieceType2, ref pieceColor2, out byte piece2);
        packedPieces = (byte)(piece1 | (piece2 << 4));
    }
    #endregion

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

    public static string GetFormattedDataWithColor(ref byte packedPiece) => $"<color=#949494>{GetType(ref packedPiece)}_{GetColor(ref packedPiece)}</color>";
    public static string GetFormattedData(ref byte packedPiece) => $"{GetType(ref packedPiece)}_{GetColor(ref packedPiece)}";

    public static bool IsEqualColor(ref byte packedPiece, PieceColor color) => GetColor(ref packedPiece) == color;
    public static void IsEqualColor(ref byte packedPiece, PieceColor color, out bool isEqual) => isEqual = GetColor(ref packedPiece) == color;

    public static bool IsEqualColor(ref byte packedPiece1, ref byte packedPiece2) => GetColor(ref packedPiece1) == GetColor(ref packedPiece2);
    public static void IsEqualColor(ref byte packedPiece1, ref byte packedPiece2, out bool isEqual) => isEqual = GetColor(ref packedPiece1) == GetColor(ref packedPiece2);


    public static bool IsEqualType(ref byte packedPiece, PieceType type) => GetType(ref packedPiece) == type;
    public static void IsEqualType(ref byte packedPiece, PieceType type, out bool isEqual) => isEqual = GetType(ref packedPiece) == type;

    public static bool IsEqualType(ref byte packedPiece1, ref byte packedPiece2) => GetType(ref packedPiece1) == GetType(ref packedPiece2);
    public static void IsEqualType(ref byte packedPiece1, ref byte packedPiece2, out bool isEqual) => isEqual = GetType(ref packedPiece1) == GetType(ref packedPiece2);

    public static bool IsDefaultPiece(ref byte packedPiece) => !IsEqualType(ref packedPiece, PieceType.None) && !IsEqualType(ref packedPiece, PieceType.Other);

    #region Not reccomended

    [Obsolete("Use GetType with out-arg")]
    public static PieceType GetType(byte packedPiece) => GetType(ref packedPiece);

    [Obsolete("Use GetColor with out-arg")]
    public static PieceColor GetColor(byte packedPiece) => GetColor(ref packedPiece);

    #endregion
}