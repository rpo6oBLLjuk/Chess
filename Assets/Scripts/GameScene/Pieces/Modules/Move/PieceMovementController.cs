using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PieceMovementController
{
    [Inject] GameController gameController;
    [Inject] PieceService pieceService;

    /// <summary>
    /// </summary>
    /// <param name="piece">Moved piece data</param>
    /// <param name="from">Start cell handler</param>
    /// <param name="to">End cell handler.</param>
    public event Action<PieceData, CellHandler, CellHandler> PieceMoved;


    public bool CanBeMove(PieceHandler pieceHandler)
    {
        PieceData endCellData = gameController.DeskData.BoardData[pieceHandler.CurrentCell.CellIndex.x, pieceHandler.CurrentCell.CellIndex.y];
        if (endCellData == null)
            return true;

        return endCellData.Color switch
        {
            PieceColor.None => true,
            PieceColor.Other => false,
            _ => endCellData.Color != pieceHandler.PieceData.Color,
        };
    }

    public void Move(PieceHandler pieceHandler)
    {
        MovePieceData(pieceHandler.PreviousCell.CellIndex, pieceHandler.CurrentCell.CellIndex);
        PieceMoved?.Invoke(pieceHandler.PieceData, pieceHandler.PreviousCell, pieceHandler.CurrentCell);
    }

    private void MovePieceData(Vector2Int startCell, Vector2Int endCell)
    {
        PieceData pieceData = gameController.DeskData.BoardData[startCell.x, startCell.y]?.Clone();
        gameController.DeskData.BoardData[startCell.x, startCell.y] = null;
        gameController.DeskData.BoardData[endCell.x, endCell.y] = pieceData;
    }
}
