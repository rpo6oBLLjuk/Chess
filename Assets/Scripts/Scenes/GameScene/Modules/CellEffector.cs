using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellEffector : MonoBehaviour
{
    [Inject] GameController gameController;

    [SerializeField] private bool selectInactive = false;

    CellHandler selectedCell;
    CellHandler lastMoveCell;

    List<CellHandler> capturedCells = new();

    List<CellHandler> possibleMoveCells = new();


    private void OnEnable()
    {
        gameController.CellPressedDown += PieceDown;
        gameController.PieceMoved += PieceMoved;

        gameController.PieceDragged += PieceDragged;

        gameController.BoardCleared += BoardCleared;
    }

    private void OnDisable()
    {
        gameController.CellPressedDown -= PieceDown;
        gameController.PieceMoved -= PieceMoved;

        gameController.PieceDragged -= PieceDragged;

        gameController.BoardCleared -= BoardCleared;
    }

    private void PieceDown(CellHandler cellHandler)
    {
        if (gameController.Board[cellHandler.Index] != 0 || selectInactive)
        {
            selectedCell?.CellEffectController.DisableSelect();
            DisablePossibleMoveCells();

            selectedCell = cellHandler;
            selectedCell.CellEffectController.EnableSelect();

            lastMoveCell?.CellEffectController.DisableLastMove();
            lastMoveCell = null;
        }

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
            if (selectedCell != cellHandler)
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

    private void BoardCleared()
    {
        DisablePossibleMoveCells();

        capturedCells.Clear();

        selectedCell?.CellEffectController.DisableSelect();
        selectedCell = null;
        lastMoveCell = null;
    }

    private void DisablePossibleMoveCells()
    {
        possibleMoveCells.ForEach(cell => cell?.CellEffectController.DisablePossibleMove());
        possibleMoveCells.Clear();
    }

    private void DisableCapturedCells()
    {
        capturedCells.ForEach(cell => cell?.CellEffectController.DisableCapture());
        capturedCells.Clear();
    }
}
