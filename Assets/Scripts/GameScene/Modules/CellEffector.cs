using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellEffector : MonoBehaviour
{
    [Inject] GameController gameController;
    [SerializeField] private BoardService boardService;

    [SerializeField] private PieceHandler lastPiece;

    [SerializeField] List<CellHandler> selectedCells = new();
    [SerializeField] List<CellHandler> possibleMoveCells = new();

    private void OnEnable()
    {
        gameController.PiecePointerDown += PieceDown;
    }

    private void OnDisable()
    {
        gameController.PiecePointerDown -= PieceDown;
    }

    private void PieceDown(PieceHandler pieceHandler)
    {
        SetColor(selectedCells, default);
        selectedCells.Clear();

        lastPiece = pieceHandler;
        selectedCells.Add(gameController.CellsData.Get(pieceHandler.CurrentCell.CellIndex));

        SetColor(selectedCells, boardService.cellsSkinData.SelectImage.Color);
    }

    private void SetColor(List<CellHandler> cells, Color color)
    {
        foreach (CellHandler cellHandler in cells)
        {
            cellHandler.CellEffectController.SetSelectColor(color);
        }
    }
}
