using System;
using Zenject;

[Serializable]
public class PieceMover
{
    [Inject] GameController gameController;


    public bool CanBeMove(CellHandler startCell, CellHandler endCell)
    {
        byte endPieceData = gameController.Board[endCell.Index];
        if (endPieceData == 0)
            return true;


        PiecePacker.GetType(ref endPieceData, out PieceType endPieceType);
        switch (endPieceType)
        {
            case PieceType.None:
            return true;
            case PieceType.Other:
            return false;
            default:
            {
                if (PiecePacker.GetColor(endPieceData) != PiecePacker.GetColor(gameController.Board[startCell.Index]))
                {
                    gameController.CapturePiece(gameController.Pieces[startCell.Index], endCell);
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

        MovePieceData(startCell.Index, endCell.Index);
    }

    private void MovePieceData(byte startIndex, byte endIndex)
    {
        (gameController.Board[startIndex], gameController.Board[endIndex]) = (gameController.Board[endIndex], gameController.Board[startIndex]);
        (gameController.Pieces[startIndex], gameController.Pieces[endIndex]) = (gameController.Pieces[endIndex], gameController.Pieces[startIndex]);
    }
}
