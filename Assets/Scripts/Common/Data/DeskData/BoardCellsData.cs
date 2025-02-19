using System;
using UnityEngine;

[Serializable]
public class BoardCellsData : ArrayToMatrixData<CellHandler>
{
    public BoardCellsData(int x, int y) : base(x, y) { }
    public BoardCellsData(Vector2Int size) : base(size) { }
}
