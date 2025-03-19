using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    List<CellHandler> destroyedCells = new();

    List<CellHandler> possibleMoveCells = new();

    private bool captured = false;


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
        DisableSelectedCell();
        DisablePossibleMoveCells();

        if (!PiecePacker.IsEqualType(gameManager.Board[cellHandler.Index], PieceType.None) || selectInactive)
        {
            EnableSelectedCell(cellHandler);
            EnablePossibleMoveCells(cellHandler.Index);

            DisablePreviousCells();
        }
        else
        {
            EnablePreviousCells(previousMoveStartCell, previousMoveEndCell);
        }
    }

    private void CellPressedUp(CellHandler cellHandler)
    {
        DisableHoverCell();
    }

    private void PieceMoved(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        DisableHoverCell();
        DisableSelectedCell();

        DisablePossibleMoveCells();
        DisableDestroyedCells();

        DisableCapturedCell();

        EnablePreviousCells(startCell, endCell);
    }

    private void PieceCaptured(PieceHandler eater, PieceHandler eaten, CellHandler cellHandler)
    {
        EnableCapturedCell(cellHandler);
    }

    private void PieceDragged(PieceHandler piece, CellHandler cellHandler)
    {
        if (hoverCell != cellHandler)
        {
            EnableHoverCell(cellHandler);
        }
    }

    private void BoardCleared()
    {
        DisablePossibleMoveCells();

        DisableCapturedCell();
        DisableSelectedCell();
        DisablePreviousCells();
    }


    private void EnableHoverCell(CellHandler cellHandler)
    {
        DisableHoverCell();
        hoverCell = cellHandler;
        hoverCell.CellEffectController.EnableAnimHover();
    }
    private void DisableHoverCell()
    {
        hoverCell?.CellEffectController.DisableAnimHover();
        hoverCell = null;
    }

    private void EnablePreviousCells(CellHandler startCell, CellHandler endCell)
    {
        DisablePreviousCells();

        previousMoveStartCell = startCell;
        previousMoveStartCell.CellEffectController.EnablePreviousMove();

        previousMoveEndCell = endCell;
        if (previousMoveEndCell != capturedCell)
            previousMoveEndCell.CellEffectController.EnablePreviousMove();
    }
    private void DisablePreviousCells()
    {
        previousMoveStartCell?.CellEffectController.DisablePreviousMove();
        previousMoveEndCell?.CellEffectController.DisablePreviousMove();
    }

    private void EnableSelectedCell(CellHandler cellHandler)
    {
        DisableSelectedCell();
        selectedCell = cellHandler;
        selectedCell?.CellEffectController.EnableSelect();
    }
    private void DisableSelectedCell()
    {
        selectedCell?.CellEffectController.DisableSelect();
        selectedCell = null;
    }

    private void EnableCapturedCell(CellHandler cellHandler)
    {
        DisableCapturedCell();

        captured = true;

        capturedCell = cellHandler;
        capturedCell.CellEffectController.EnableCapture();
    }
    private void DisableCapturedCell()
    {
        if (captured)
        {
            captured = false;
            return;
        }

        capturedCell?.CellEffectController?.DisableCapture();
        capturedCell = null;
    }

    private void EnablePossibleMoveCells(int index)
    {
        DisablePossibleMoveCells();

        foreach (byte possibleMoveCell in gameManager.Moves[index])
        {
            CellHandler cellHandler = gameManager.Cells[possibleMoveCell];
            possibleMoveCells.Add(cellHandler);
            cellHandler?.CellEffectController?.EnablePossibleMove();
        }
    }
    private void DisablePossibleMoveCells()
    {
        possibleMoveCells.ForEach(cell => cell?.CellEffectController.DisablePossibleMove());
        possibleMoveCells.Clear();
    }

    private void EnableDestroyCells(List<CellHandler> cellHandlers)
    {
        DisableDestroyedCells();

        destroyedCells = cellHandlers;
    }
    private void DisableDestroyedCells()
    {
        destroyedCells.ForEach(cell => cell?.CellEffectController.DisableCapture());
        destroyedCells.Clear();
    }
}
