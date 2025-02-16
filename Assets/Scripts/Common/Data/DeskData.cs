using System;
using UnityEngine;

[Serializable]
public class DeskData
{
    public Vector2Int BoardSize = new(8, 8);
    public PieceData[,] BoardData
    {
        get => pieceData ??= new PieceData[BoardSize.x, BoardSize.y];
        private set => pieceData = value;
    }
    private PieceData[,] pieceData;
}
