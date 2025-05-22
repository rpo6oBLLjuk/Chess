using ModestTree;
using System;
using System.Linq;
using Zenject;

[Serializable]
public class PieceMover
{
    [Inject] GameManager gameManager;

    private CellHandler previousCell;


    public void Init()
    {
        gameManager.CellClicked += ClickOnCell;
        gameManager.PieceDragEnded += DragEnded;
    }

    public void OnDisable()
    {
        gameManager.CellClicked -= ClickOnCell;
        gameManager.PieceDragEnded -= DragEnded;
    }

    private void ClickOnCell(CellHandler clickedCellHandler)
    {
        if (previousCell != null)
        {
            PieceHandler movingPiece = gameManager.Pieces[previousCell.Index];

            if (MoveAttempt(movingPiece, previousCell, clickedCellHandler))
            {
                gameManager.MovePiece(movingPiece, previousCell, clickedCellHandler);
                previousCell = null;
            }
            else if (PiecePacker.IsDefaultPiece(gameManager.Board[clickedCellHandler.Index]) &&
                PiecePacker.IsEqualColor(gameManager.Board[previousCell.Index], gameManager.Board[clickedCellHandler.Index]) &&
                gameManager.PossibleMoves[clickedCellHandler.Index].Count() > 0)
            {
                previousCell = clickedCellHandler;
            }
            else
            {
                previousCell = null;
            }
        }
        else
        {
            byte clickedPiece = gameManager.Board[clickedCellHandler.Index];
            if (PiecePacker.IsDefaultPiece(clickedPiece) && PiecePacker.IsEqualColor(clickedPiece, gameManager.GameTurnController.TurnColor) && gameManager.PossibleMoves[clickedCellHandler.Index].Count() > 0)
                previousCell = clickedCellHandler;
        }
    }

    private void DragEnded(PieceHandler movingPiece, CellHandler endCell)
    {
        CellHandler startCell = gameManager.Cells[gameManager.Pieces.IndexOf(movingPiece)];
        if (MoveAttempt(movingPiece, startCell, endCell))
        {
            gameManager.MovePiece(movingPiece, startCell, endCell);
        }
        else
        {
            gameManager.BlockPieceMove(movingPiece, startCell, endCell);
        }
    }

    private bool MoveAttempt(PieceHandler movingPiece, CellHandler startCell, CellHandler endCell)
    {
        if (endCell != startCell)
        {
            if (gameManager.IsMoveAllowed(movingPiece, startCell, endCell))
            {
                return true;
            }
        }
        return false;
    }
}
