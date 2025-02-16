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

    private PieceService pieceService;


    public void Init(PieceService pieceService)
    {
        this.pieceService = pieceService;
    }

    public void Move(PieceHandler pieceHandler, CellHandler endCellHandler)
    {
        CellHandler startCellHandler = pieceHandler.PreviousCell;

        PieceData pieceData = pieceService.DeskData.PieceData[startCellHandler.CellIndex.x, startCellHandler.CellIndex.y]?.Clone();
        pieceService.DeskData.PieceData[startCellHandler.CellIndex.x, startCellHandler.CellIndex.y] = null;
        pieceService.DeskData.PieceData[endCellHandler.CellIndex.x, endCellHandler.CellIndex.y] = pieceData;
    }
}
