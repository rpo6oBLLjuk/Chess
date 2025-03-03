using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellEffector : MonoBehaviour
{
    [Inject] GameController gameController;

    CellHandler startSelectCell;
    CellHandler lastMoveCell;

    List<CellHandler> capturedCells;

    List<CellHandler> possibleMoveCells = new();


    private void OnEnable()
    {
        gameController.CellPressedDown += PieceDown;
        gameController.PieceMoved += PieceMoved;

        gameController.PieceDragged += PieceDragged;
    }

    private void OnDisable()
    {
        gameController.CellPressedDown -= PieceDown;
        gameController.PieceMoved -= PieceMoved;

        gameController.PieceDragged -= PieceDragged;
    }

    private void PieceDown(CellHandler cellHandler)
    {
        startSelectCell?.CellEffectController.DisableSelect();
        DisablePossibleMoveCells();

        startSelectCell = cellHandler;
        startSelectCell.CellEffectController.EnableSelect();

        lastMoveCell?.CellEffectController.DisableLastMove();
        lastMoveCell = null;


        //CellHandler testPossible = gameController.CellsData.Get(cellHandler.CellIndex + Vector2Int.up);
        //testPossible.CellEffectController.EnablePossibleMove();
        //possibleMoveCells.Add(testPossible);

        //CellHandler testCaptureCell = gameController.CellsData.Get(cellHandler.CellIndex - Vector2Int.up);
        //testCaptureCell.CellEffectController.EnableCapture();
    }

    private void PieceMoved(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        //startSelectCell?.CellEffectController.SetSelectColor(default);
        DisablePossibleMoveCells();
    }

    private void PieceDragged(PieceHandler piece, CellHandler cellHandler)
    {
        if (lastMoveCell != cellHandler)
        {
            lastMoveCell?.CellEffectController.DisableLastMove();
            if (startSelectCell != cellHandler)
            {
                lastMoveCell = cellHandler;
                lastMoveCell.CellEffectController.EnableLastMove();
            }
            else
            {
                lastMoveCell = null;
            }
        }
    }


    private void DisablePossibleMoveCells()
    {
        possibleMoveCells.ForEach(cell => cell.CellEffectController.DisablePossibleMove());
        possibleMoveCells.Clear();
    }
}
