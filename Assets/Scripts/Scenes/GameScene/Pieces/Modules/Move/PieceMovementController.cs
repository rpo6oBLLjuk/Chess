using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PieceMovementController
{
    [Inject] GameController gameController;


    public bool CanBeMove(CellHandler startCell, CellHandler endCell)
    {
        byte endPieceData = gameController.Pieces[endCell.CellIndex];
        if (endPieceData == 0)
            return true;


        PiecePacker.GetPieceType(ref endPieceData, out PieceType endPieceType);
        switch (endPieceType)
        {
            case PieceType.None:
            return true;
            case PieceType.Other:
            return false;
            default:
            {
                if (PiecePacker.GetPieceColor(endPieceData) != PiecePacker.GetPieceColor(gameController.Pieces[startCell.CellIndex]))
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

    private void MovePieceData(byte startIndex, byte endIndex)
    {
        (gameController.Pieces[startIndex], gameController.Pieces[endIndex]) = (gameController.Pieces[endIndex], gameController.Pieces[startIndex]);
    }
}
