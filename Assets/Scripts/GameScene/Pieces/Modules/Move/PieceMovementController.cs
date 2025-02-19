using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PieceMovementController
{
    [Inject] GameController gameController;


    public bool CanBeMove(PieceHandler pieceHandler)
    {
        PieceData endPieceData = gameController.PiecesData.Get(pieceHandler.CurrentCell.CellIndex);
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
                    gameController.EatPiece(pieceHandler.CurrentCell);
                    return true;
                }
                else
                    return false;
            }
        }
    }

    public void Move(PieceHandler pieceHandler)
    {
        MovePieceData(pieceHandler.PreviousCell.CellIndex, pieceHandler.CurrentCell.CellIndex);
    }

    private void MovePieceData(Vector2Int startCell, Vector2Int endCell)
    {
        PieceData pieceData = gameController.PiecesData.Get(startCell)?.Clone();
        gameController.PiecesData.Set(startCell, new PieceData());
        gameController.PiecesData.Set(endCell, pieceData);
    }
}
