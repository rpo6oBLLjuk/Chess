using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPressedDown;

    public event Action<PieceHandler, CellHandler> PieceDragged;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceCaptured;
    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    [field: SerializeField] public BoardPiecesData PiecesData { get; private set; }
    [field: SerializeField] public BoardCellsData CellsData { get; private set; }

    public CellsSkinData CellsSkinData => boardService.cellsSkinData;
    public PiecesSkinData PiecesSkinData => pieceService.piecesSkinData;

    [Inject] DeskSaverService deskSaver;
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;


    public void Setup()
    {
        pieceService.OnInstantiated();
        boardService.OnInstantiated();

        PiecesData = new BoardPiecesData(8, 8);
        PiecesData.SetDefaultBoard();

        SetupServices();
    }

    public void ClearBoard()
    {
        pieceService.ClearBoard();
        Debug.Log("Board cleared");
    }

    public void SetCustomBoard(BoardPiecesData boardPiecesData)
    {
        PiecesData = boardPiecesData;
        SetupServices();

        Debug.Log($"Board created, Size: {PiecesData.Size}");
    }

    public void SpawnPiece(PieceData pieceData, CellHandler cellHandler) => pieceService.SpawnPiece(pieceData, cellHandler);
    public void SpawnPiece(PieceType pieceType, PieceColor pieceColor, CellHandler cellHandler) => pieceService.SpawnPiece(pieceType, pieceColor, cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.CanBeMove(pieceHandler, startCell, endCell);

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        PieceMoved?.Invoke(pieceHandler, startCell, endCell);

        Debug.Log($"Piece ({pieceHandler.PieceData.Type}_{pieceHandler.PieceData.Color}) moved from {startCell.CellIndex} to {endCell.CellIndex}");
    }
    public void CapturePiece(CellHandler captiredCellHandler)
    {
        pieceService.CapturePiece(captiredCellHandler);

        Debug.Log($"Piece captired on cell {captiredCellHandler.CellIndex}");
    }

    public void DestroyPiece(CellHandler cellHandler)
    {
        pieceService.CapturePiece(cellHandler);
        Debug.Log($"Piece destroyed on cell {cellHandler.CellIndex}");
    }

    public void ClickOnCell(CellHandler cellHandler)
    {
        CellClicked?.Invoke(cellHandler);

        Debug.Log($"Cell {cellHandler.CellIndex} clicked");
    }
    public void PressDownOnCell(CellHandler cellHandler)
    {
        CellPressedDown?.Invoke(cellHandler);

        Debug.Log($"Cell {cellHandler.CellIndex} pressed down ");
    }

    public void PieceDragging(PieceHandler piece, CellHandler downCell)
    {
        PieceDragged?.Invoke(piece, downCell);

        //Debug.Log($"Dragging piece across ({downCell.CellIndex}) cell");
    }

    private void SetupServices()
    {
        boardService.Setup();
        pieceService.Setup();
    }
}
