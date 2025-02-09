using System;
using UnityEngine;

[Serializable]
public class DeskData
{
    public Vector2Int boardSize = new(8, 8);
    public PieceData[,] PieceData;
}
