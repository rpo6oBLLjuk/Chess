using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardPiecesData
{
    public Vector2Int Size => size;
    [SerializeField] private Vector2Int size;

    public List<PieceData> Data => data;
    [SerializeReference] private List<PieceData> data = new();

    public BoardPiecesData()
    {
        ResetDataList();
    }
    public BoardPiecesData(int x, int y) { SetSize(new Vector2Int(x, y)); }
    public BoardPiecesData(Vector2Int size) { SetSize(size); }

    public virtual void SetSize(Vector2Int size)
    {
        this.size = size;

        ResetDataList();
    }
    private void ResetDataList()
    {
        data?.Clear();
        data = new List<PieceData>(size.x * size.y);

        for (int i = 0; i < size.x * size.y; i++)
            data.Add(new PieceData());
    }

    public void Set(int x, int y, PieceData instance) => data[y * size.x + x] = instance;
    public void Set(Vector2Int index, PieceData instance) => Set(index.x, index.y, instance);

    public PieceData Get(int x, int y) => data[y * size.x + x];
    public PieceData Get(Vector2Int index) => Get(index.x, index.y);
}
