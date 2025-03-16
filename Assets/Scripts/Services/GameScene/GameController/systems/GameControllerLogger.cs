using UnityEngine;
using Zenject;

public class GameControllerLogger : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    [Header("Main Condition")]
    [SerializeField] private bool logging = true;

    [Header("Specifying Condition")]
    [SerializeField] private bool logCellActions = false;
    [SerializeField] private bool logPieceActions = true;
    [SerializeField] private bool logBoardActions = false;

    [Header("Other Condition")]
    [SerializeField] private bool logPieceDragging = false;


    private void OnEnable()
    {
        gameManager.CellClicked += CellClicked;
        gameManager.CellPressedDown += CellPressedDown;
        gameManager.PieceDragged += PieceDragged;
        gameManager.PieceMoved += PieceMoved;
        gameManager.PieceCaptured += PieceCaptured;
        gameManager.PieceDestroyed += PieceDestroyed;
        gameManager.BoardLoaded += BoardLoaded;
        gameManager.BoardCleared += BoardCleared;
    }

    private void OnDisable()
    {
        gameManager.CellClicked -= CellClicked;
        gameManager.CellPressedDown -= CellPressedDown;
        gameManager.PieceDragged -= PieceDragged;
        gameManager.PieceMoved -= PieceMoved;
        gameManager.PieceCaptured -= PieceCaptured;
        gameManager.PieceDestroyed -= PieceDestroyed;
        gameManager.BoardLoaded -= BoardLoaded;
        gameManager.BoardCleared -= BoardCleared;
    }

    private void CellClicked(CellHandler cellHandler) => CellLog($"Cell {cellHandler.Index} clicked");
    private void CellPressedDown(CellHandler cellHandler) => CellLog($"Cell {cellHandler.Index} pressed down");

    private void PieceDragged(PieceHandler piece, CellHandler downCell) => SendLog($"Dragging piece across ({downCell.Index}) cell", logPieceDragging);

    private void PieceMoved(PieceHandler piece, CellHandler startCell, CellHandler endCell) => PieceLog($"Piece moved from {startCell.Index} to {endCell.Index}");
    private void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, CellHandler cell) => PieceLog($"Piece captured on cell {cell.Index}");
    private void PieceDestroyed(PieceHandler capturerPiece, CellHandler cell) => PieceLog($"Piece destroyed on cell {cell.Index}");

    private void BoardCleared() => BoardLog("Board cleared");
    private void BoardLoaded() => BoardLog("Board loaded");

    private void CellLog(string message) => SendLog(message, logCellActions);
    private void PieceLog(string message) => SendLog(message, logPieceActions);
    private void BoardLog(string message) => SendLog(message, logBoardActions);

    private void SendLog(string message, bool secondCondition)
    {
        if (logging && secondCondition)
            this.Log(message, "GameController");
    }
}