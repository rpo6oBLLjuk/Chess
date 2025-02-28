using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PieceMovementController
{
    [Inject] GameController gameController;


    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        PieceData endPieceData = gameController.PiecesData.Get(endCell.CellIndex);
        if (endPieceData == new PieceData())
            return true;

        switch (endPieceData.Color)
        {
            case PieceColor.None:
            return true;
            case PieceColor.Other:
            return false;
            default:
            {
                if (endPieceData.Color != pieceHandler.PieceData.Color)
                {
                    gameController.CapturePiece(endCell);
                    return true;
                }
                else
                    return false;
            }
        }
    }

    public void Move(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        startCell.PieceRemoved();
        endCell.PiecePlaced(pieceHandler);

        MovePieceData(startCell.CellIndex, endCell.CellIndex);
    }

    private void MovePieceData(Vector2Int startCell, Vector2Int endCell)
    {
        PieceData pieceData = gameController.PiecesData.Get(startCell)?.Clone();
        gameController.PiecesData.Set(startCell, new PieceData());
        gameController.PiecesData.Set(endCell, pieceData);
    }
}
