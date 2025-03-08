using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArrayToMatrixData<T>
{
    public Vector2Int Size => size;
    [SerializeField] private Vector2Int size;

    public List<T> Data => data;
    [SerializeReference] private List<T> data = new();

    public ArrayToMatrixData(byte x, byte y) { SetSize(x, y); }

    public virtual void SetSize(byte width, byte height)
    {
        this.size = new(width, height);
        data = new List<T>(size.x * size.y);
    }

    public void Set(byte x, byte y, T instance)
    {
        Debug.Log($"Index: x = {x}, y = {y}");
        data[y * size.x + x] = instance;
    }
    public void Set(byte index, T instance) => data[index] = instance;

    public T Get(byte x, byte y) => data[y * size.x + x];
    public T Get(byte index) => data[index];
}