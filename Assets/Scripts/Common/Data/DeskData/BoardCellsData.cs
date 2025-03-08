using System;

[Serializable]
public class BoardCellsData : Grid<CellHandler>
{
    public BoardCellsData(byte x, byte y) : base(x, y) { }
}
