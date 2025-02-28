using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellEffector : MonoBehaviour
{
    [Inject] GameController gameController;
    [SerializeField] private BoardService boardService;

    CellHandler selectCells;

    List<CellHandler> possibleMoveCells = new();
    

    private void OnEnable()
    {
        gameController.CellPointerDown += PieceDown;
        gameController.PieceMoved += PieceMoved;
    }

    private void OnDisable()
    {
        gameController.CellPointerDown -= PieceDown;
        gameController.PieceMoved -= PieceMoved;
    }

    private void PieceDown(CellHandler cellHandler)
    {
        selectCells?.CellEffectController.SetSelectColor(default);
        DisablePossibleMoveCells();

        selectCells = cellHandler;
        selectCells.CellEffectController.SetSelectColor(boardService.cellsSkinData.SelectImage.Color);
    }

    private void PieceMoved(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        selectCells?.CellEffectController.SetSelectColor(default);
        DisablePossibleMoveCells();
    }


    private void DisablePossibleMoveCells() => DisableCells(possibleMoveCells);

    private void DisableCells(List<CellHandler> cells)
    {
        SetColor(cells, default);
        cells.Clear();
    }

    private void SetColor(List<CellHandler> cells, Color color) => cells.ForEach(cell => cell.CellEffectController.SetSelectColor(color));
}
