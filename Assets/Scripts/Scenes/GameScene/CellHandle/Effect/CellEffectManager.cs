using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CellEffectManager : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] private bool selectInactive = false;


    CellHandler selectedCell;

    CellHandler previousMoveStartCell;
    CellHandler previousMoveEndCell;

    CellHandler hoverCell;

    CellHandler capturedCell;
    List<CellHandler> capturedCells = new();

    List<CellHandler> possibleMoveCells = new();


    private void OnEnable()
    {
        gameManager.CellPressedDown += CellPressedDown;
        gameManager.CellPressedUp += CellPressedUp;

        gameManager.PieceMoved += PieceMoved;
        gameManager.PieceCaptured += PieceCaptured;

        gameManager.PieceDragged += PieceDragged;

        gameManager.BoardCleared += BoardCleared;
    }

    private void OnDisable()
    {
        gameManager.CellPressedDown -= CellPressedDown;
        gameManager.CellPressedUp -= CellPressedUp;

        gameManager.PieceMoved -= PieceMoved;
        gameManager.PieceCaptured -= PieceCaptured;

        gameManager.PieceDragged -= PieceDragged;

        gameManager.BoardCleared -= BoardCleared;
    }

    private void CellPressedDown(CellHandler cellHandler)
    {
        selectedCell?.CellEffectController.DisableSelect();
        DisablePossibleMoveCells();

        if (!PiecePacker.IsEqualType(ref gameManager.Board[cellHandler.Index], PieceType.None) || selectInactive)
        {
            selectedCell = cellHandler;
            selectedCell.CellEffectController.EnableSelect();
        }
    }

    private void CellPressedUp(CellHandler cellHandler)
    {
        hoverCell?.CellEffectController.DisableAnimHover();
        hoverCell = null;
    }

    private void PieceMoved(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        hoverCell?.CellEffectController.DisableAnimHover();
        hoverCell = null;

        selectedCell?.CellEffectController.DisableSelect();
        selectedCell = null;

        DisablePossibleMoveCells();
        DisableCapturedCells(capturedCell);

        previousMoveStartCell?.CellEffectController.DisablePreviousMove();
        previousMoveStartCell = startCell;
        previousMoveStartCell.CellEffectController.EnablePreviousMove();

        if (previousMoveEndCell != null && previousMoveEndCell != startCell)
            previousMoveEndCell?.CellEffectController.DisablePreviousMove();
        previousMoveEndCell = endCell;
        if (previousMoveEndCell != capturedCell)
            previousMoveEndCell.CellEffectController.EnablePreviousMove();

        capturedCell = null;
    }

    private void PieceCaptured(PieceHandler eater, PieceHandler eaten, CellHandler cellHandler)
    {
        DisableCapturedCells();
        capturedCells.Add(cellHandler);
        capturedCell = cellHandler;
        cellHandler?.CellEffectController.ChangeAll(enableCaptured: true);
    }

    private void PieceDragged(PieceHandler piece, CellHandler cellHandler)
    {
        if (hoverCell != cellHandler)
        {
            hoverCell?.CellEffectController.DisableAnimHover();
            hoverCell = cellHandler;
            hoverCell.CellEffectController.EnableAnimHover();
        }
    }

    private void BoardCleared()
    {
        DisablePossibleMoveCells();

        capturedCells.Clear();

        selectedCell?.CellEffectController.DisableSelect();
        selectedCell = null;
        previousMoveStartCell = null;
        previousMoveEndCell = null;
    }

    private void DisablePossibleMoveCells()
    {
        possibleMoveCells.ForEach(cell => cell?.CellEffectController.DisablePossibleMove());
        possibleMoveCells.Clear();
    }

    private void DisableCapturedCells(CellHandler ignoredCell = null)
    {
        capturedCells.Where(cell => cell != ignoredCell).ToList().ForEach(cell => cell?.CellEffectController.DisableCapture());
        //capturedCells.Clear();
    }
}
