using System;
using UnityEngine;

public class CellHandler : MonoBehaviour
{
    public Action<Vector2Int, GameObject> OnPiecePlaced;

    public Vector2Int cellIndex;


    public void Init(int x, int y)
    {
        cellIndex = new Vector2Int(x, y);
    }

    public void PiecePlaced(GameObject Piece)
    {
        OnPiecePlaced?.Invoke(cellIndex, Piece);
    }
}
