using UnityEngine;

public class CellHandler : MonoBehaviour
{
    public Vector2Int CellIndex { get; private set; }
    public PieceHandler CellPieceHandler { get; private set; }

    public CellEffectController CellEffectController { get; private set; }


    public void Init(int x, int y)
    {
        CellIndex = new Vector2Int(x, y);
        CellEffectController ??= GetComponentInChildren<CellEffectController>();
    }

    public void PiecePlaced(PieceHandler pieceHandler)
    {
        CellPieceHandler = pieceHandler;
        Debug.Log($"CellHandler: {this.gameObject.name}, PieceHandler: {pieceHandler.gameObject}");
    }
    public void PieceMovedFrom(PieceHandler pieceHandler)
    {
        CellPieceHandler = null;
    }
}
