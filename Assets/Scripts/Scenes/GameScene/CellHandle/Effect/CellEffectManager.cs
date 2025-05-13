using ModestTree;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellEffectManager : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] CellEffectManagerData data;


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
        gameManager.PieceDragEnded += PieceDragEnd;

        gameManager.PieceMoved += PieceMoved;
        gameManager.PieceCaptured += PieceCaptured;

        gameManager.PieceDragged += PieceDragged;

        gameManager.BoardCleared += BoardCleared;
    }

    private void OnDisable()
    {
        gameManager.CellPressedDown -= CellPressedDown;
        gameManager.PieceDragEnded -= PieceDragEnd;

        gameManager.PieceMoved -= PieceMoved;
        gameManager.PieceCaptured -= PieceCaptured;

        gameManager.PieceDragged -= PieceDragged;

        gameManager.BoardCleared -= BoardCleared;
    }

    private void CellPressedDown(CellHandler cellHandler)
    {
        DisableSelectedCell();
        DisablePossibleMoveCells();

        if (gameManager.Moves[cellHandler.Index].Count > 0 || data.SelectInactiveCells)
        {
            EnableSelectedCell(cellHandler);
            EnablePossibleMoveCells(cellHandler.Index);

            if (data.DisablePreviousMoveCellsBeforeSelect)
            {
                DisablePreviousCells();
            }
            if (data.DisableCapturedCellsBeforeSelect)
            {
                DisableCapturedCell();
            }
        }
        else
        {
            EnablePreviousCells(previousMoveStartCell, previousMoveEndCell);
        }
    }

    private void PieceDragEnd(PieceHandler _, Transform __)
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

    private void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, byte capturedPieceData, CellHandler cellHandler)
    {
        EnableCapturedCell(cellHandler);
    }

    private void PieceDragged(PieceHandler piece, Vector3 _, CellHandler cellHandler)
    {
        if (hoverCell != cellHandler && (gameManager.Moves[gameManager.Pieces.IndexOf(piece)].Count > 0 || /*CellEffector.data.DragInactivePiece*/false))
            EnableHoverCell(cellHandler);
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
        previousMoveStartCell?.CellEffectController.EnablePreviousMove();

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

        if (gameManager.Moves[index] == null)
            return;

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
