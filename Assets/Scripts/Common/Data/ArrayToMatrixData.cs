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

    public ArrayToMatrixData(int x, int y) : this(new Vector2Int(x, y)) { }
    public ArrayToMatrixData(Vector2Int size) { SetSize(size); }

    public virtual void SetSize(Vector2Int size, T defaultInstance = default)
    {
        this.size = size;
        data = new List<T>(size.x * size.y);
        for (int i = 0; i < size.x * size.y; i++)
            data.Add(defaultInstance);
    }

    public void Set(int x, int y, T instance) => data[y * size.x + x] = instance;
    public void Set(Vector2Int index, T instance) => Set(index.x, index.y, instance);

    public T Get(int x, int y) => data[y * size.x + x];
    public T Get(Vector2Int index) => Get(index.x, index.y);
}