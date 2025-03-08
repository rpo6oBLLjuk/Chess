using System;
using UnityEngine;

public class Grid<T>
{
    public T[] Array;

    public byte Size => size;
    [SerializeField] private byte size;

    public byte Width => (byte)(Size & 0x0F);
    public byte Height => (byte)((Size >> 4) & 0x0F);


    public Grid(byte width, byte height)
    {
        if (width > 16 || height > 16)
            throw new ArgumentException("Width and height must be <= 16.");

        size = (byte)((height << 4) | width);
        Array = new T[Width * Height];

    }

    public T this[int x, int y]
    {
        get => Array[y * Width + x];
        set => Array[y * Width + x] = value;
    }

    public T this[int index]
    {
        get => Array[index];
        set => Array[index] = value;
    }
}