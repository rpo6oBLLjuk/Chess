using CustomInspector;
using UnityEngine;
using Zenject;

public class GameManagerLogger : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [HorizontalLine("Main Condition")]
    [SerializeField] bool logging = true;

    [HorizontalLine(1, FixedColor.Black, 7, message = "Specifying Condition")]
    [SerializeField, ShowIf(nameof(logging))] bool logCellActions = false;
    [SerializeField, ShowIf(nameof(logging))] bool logPieceDragActions = false;
    [SerializeField, ShowIf(nameof(logging))] bool logPieceActions = true;
    [SerializeField, ShowIf(nameof(logging))] bool logBoardActions = false;


    private void OnEnable()
    {
        gameManager.CellClicked += CellClicked;
        gameManager.CellPressedDown += CellPressedDown;

        gameManager.PieceDragStarted += PieceDragStart;
        gameManager.PieceDragged += PieceDragged;
        gameManager.PieceDragEnded += PieceDragEnd;

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

        gameManager.PieceDragStarted -= PieceDragStart;
        gameManager.PieceDragged -= PieceDragged;
        gameManager.PieceDragEnded -= PieceDragEnd;

        gameManager.PieceMoved -= PieceMoved;
        gameManager.PieceCaptured -= PieceCaptured;
        gameManager.PieceDestroyed -= PieceDestroyed;

        gameManager.BoardLoaded -= BoardLoaded;
        gameManager.BoardCleared -= BoardCleared;
    }

    private void CellClicked(CellHandler cellHandler) => CellLog($"Cell {cellHandler.Index} clicked");
    private void CellPressedDown(CellHandler cellHandler) => CellLog($"Cell {cellHandler.Index} pressed down");

    private void PieceDragStart(PieceHandler pieceHandler) => PieceDragLog($"Drag start for piece {pieceHandler.name}");
    private void PieceDragged(PieceHandler piece, Vector3 position, CellHandler downCell) => PieceDragLog($"Dragging piece across ({downCell?.Index}) cell at {position} position");
    private void PieceDragEnd(PieceHandler pieceHandler, Transform parent) => PieceDragLog($"Drag end for piece {pieceHandler.name}, parent: {parent.name}");

    private void PieceMoved(PieceHandler piece, CellHandler startCell, CellHandler endCell) => PieceLog($"Piece moved from {startCell.Index} to {endCell.Index}");
    private void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, byte capturedPieceData, CellHandler cell) => PieceLog($"Piece captured on cell {cell.Index}");
    private void PieceDestroyed(PieceHandler capturerPiece, CellHandler cell) => PieceLog($"Piece destroyed on cell {cell.Index}");

    private void BoardCleared() => BoardLog("Board cleared");
    private void BoardLoaded() => BoardLog("Board loaded");

    private void CellLog(string message) => SendLog(message, logCellActions);
    private void PieceDragLog(string message) => SendLog(message, logPieceDragActions);
    private void PieceLog(string message) => SendLog(message, logPieceActions);
    private void BoardLog(string message) => SendLog(message, logBoardActions);

    private void SendLog(string message, bool secondCondition)
    {
        if (logging && secondCondition)
            this.Log(message, "GameController");
    }
}