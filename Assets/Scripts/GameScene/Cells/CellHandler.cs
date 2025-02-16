using System;
using UnityEngine;

public class CellHandler : MonoBehaviour
{
    public Action<CellHandler, PieceHandler> OnPiecePlaced;

    public Vector2Int CellIndex { get; private set; }


    public void Init(int x, int y) => CellIndex = new Vector2Int(x, y);

    public void PiecePlaced(PieceHandler pieceHandler) => OnPiecePlaced?.Invoke(this, pieceHandler);
}
