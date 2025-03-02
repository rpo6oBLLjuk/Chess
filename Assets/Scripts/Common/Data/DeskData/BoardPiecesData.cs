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

    public BoardPiecesData(int x, int y) { SetSize(new Vector2Int(x, y)); }
    public BoardPiecesData(Vector2Int size) { SetSize(size); }

    public void SetDefaultBoard()
    {
        if (size != new Vector2Int(8, 8))
            ResetDataList(new(8, 8));

        SetPiece(0, 0, new PieceData(PieceType.Rook, PieceColor.Black));
        SetPiece(7, 0, new PieceData(PieceType.Rook, PieceColor.Black));
        SetPiece(0, 7, new PieceData(PieceType.Rook, PieceColor.White));
        SetPiece(7, 7, new PieceData(PieceType.Rook, PieceColor.White));

        SetPiece(1, 0, new PieceData(PieceType.Knight, PieceColor.Black));
        SetPiece(6, 0, new PieceData(PieceType.Knight, PieceColor.Black));
        SetPiece(1, 7, new PieceData(PieceType.Knight, PieceColor.White));
        SetPiece(6, 7, new PieceData(PieceType.Knight, PieceColor.White));

        SetPiece(2, 0, new PieceData(PieceType.Bishop, PieceColor.Black));
        SetPiece(5, 0, new PieceData(PieceType.Bishop, PieceColor.Black));
        SetPiece(2, 7, new PieceData(PieceType.Bishop, PieceColor.White));
        SetPiece(5, 7, new PieceData(PieceType.Bishop, PieceColor.White));

        SetPiece(3, 0, new PieceData(PieceType.Queen, PieceColor.Black));
        SetPiece(3, 7, new PieceData(PieceType.Queen, PieceColor.White));

        SetPiece(4, 0, new PieceData(PieceType.King, PieceColor.Black));
        SetPiece(4, 7, new PieceData(PieceType.King, PieceColor.White));

        for (int i = 0; i < 8; i++)
        {
            SetPiece(i, 1, new PieceData(PieceType.Pawn, PieceColor.Black));
            SetPiece(i, 6, new PieceData(PieceType.Pawn, PieceColor.White));
        }

        Debug.Log("Default data loaded");
    }

    public virtual void SetSize(Vector2Int size) => ResetDataList(size);


    public void SetPiece(int x, int y, PieceData pieceData) => data[y * size.x + x] = pieceData;
    public void SetPiece(Vector2Int index, PieceData pieceData) => SetPiece(index.x, index.y, pieceData);

    public PieceData GetPiece(int x, int y) => data[y * size.x + x];
    public PieceData GetPiece(Vector2Int index) => GetPiece(index.x, index.y);

    private void ResetDataList(Vector2Int size)
    {
        this.size = size;

        data?.Clear();
        data = new List<PieceData>(size.x * size.y);

        for (int i = 0; i < size.x * size.y; i++)
            data.Add(new PieceData());
    }
}
