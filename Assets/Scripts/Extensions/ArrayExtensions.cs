public static class ArrayWrapper
{
    public static void ConvertIndexToCoordinate(ref byte index, out byte x, out byte y)
    {
        x = (byte)(index % 8);
        y = (byte)(index / 8);
    }

    public static (byte, byte) ConvertIndexToCoordinate(ref byte index) => ((byte)(index % 8), (byte)(index / 8));


    public static byte ConvertCoordinateToIndex(byte x, byte y) => (byte)(y * 8 + x);
    public static byte ConvertCoordinateToIndex(ref byte x, ref byte y) => (byte)(y * 8 + x);
    public static void ConvertCoordinateToIndex(ref byte x, ref byte y, out byte index) => index = (byte)(y * 8 + x);

}