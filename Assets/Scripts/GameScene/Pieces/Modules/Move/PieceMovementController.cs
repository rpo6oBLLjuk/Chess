using System;
using System.Linq;

[Serializable]
public class PieceMovementController
{
    /// <summary>
    /// </summary>
    /// <param name="piece">Moved piece data</param>
    /// <param name="from">Start cell handler</param>
    /// <param name="to">End cell handler.</param>
    public event Action<PieceData, CellHandler, CellHandler> PieceMoved;

    private PieceManager pieceManager;


    public void Init(PieceManager pieceManager)
    {
        this.pieceManager = pieceManager;
    }

    public void Move(PieceHandler pieceHandler, CellHandler endCellHandler)
    {
        CellHandler startCellHandler = pieceHandler.PreviousCell;

        PieceData pieceData = pieceManager.DeskData.PieceData[startCellHandler.CellIndex.x, startCellHandler.CellIndex.y]?.Clone();
        pieceManager.DeskData.PieceData[startCellHandler.CellIndex.x, startCellHandler.CellIndex.y] = null;
        pieceManager.DeskData.PieceData[endCellHandler.CellIndex.x, endCellHandler.CellIndex.y] = pieceData;
    }
}
