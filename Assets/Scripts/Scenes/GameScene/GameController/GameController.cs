using System;
using UnityEngine;

public class GameController : MonoService
{
    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPressedDown;

    public event Action<PieceHandler, CellHandler> PieceDragged;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    public event Action<PieceHandler, byte, CellHandler> PieceCaptured;
    public event Action<byte, CellHandler> PieceDestroyed;

    public event Action BoardLoaded;
    public event Action BoardCleared;

    [Header("Grids")]
    [field: SerializeField] public Grid<byte> Board { get; private set; }

    [field: SerializeField] public Grid<PieceHandler> Pieces { get; set; }
    [field: SerializeField] public Grid<CellHandler> Cells { get; set; }

    public GameData GameData => gameData;
    public CellsSkinData CellsSkinData => boardService.cellsSkinData;
    public PiecesSkinData PiecesSkinData => pieceService.piecesSkinData;

    [Header("Dependencies")]
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;

    [Header("Data")]
    [SerializeField] private GameData gameData;


    public void Setup()
    {
        pieceService.OnInstantiated();
        boardService.OnInstantiated();

        Board = new(8, 8);

        SetupServices();
    }

    public void ClearBoard()
    {
        pieceService.ClearBoard();
        BoardCleared?.Invoke();
    }

    public void SetCustomBoard(Grid<byte> boardPiecesData)
    {
        ClearBoard();

        Board = boardPiecesData;
        SetupServices();
    }

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceService.SpawnPiece(pieceData, cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.CanBeMove(pieceHandler, startCell, endCell);

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        PieceMoved?.Invoke(pieceHandler, startCell, endCell);
    }
    public void CapturePiece(PieceHandler pieceHandler, CellHandler cellHandler)
    {
        byte destroyedPieceData = Board[cellHandler.Index];

        pieceService.CapturePiece(cellHandler);
        PieceCaptured?.Invoke(pieceHandler, destroyedPieceData, cellHandler);
    }

    public void DestroyPiece(CellHandler cellHandler)
    {
        byte destroyedPieceData = Board[cellHandler.Index];
        pieceService.CapturePiece(cellHandler);

        PieceDestroyed?.Invoke(destroyedPieceData, cellHandler);
    }

    public void ClickOnCell(CellHandler cellHandler) => CellClicked?.Invoke(cellHandler);
    public void PressDownOnCell(CellHandler cellHandler) => CellPressedDown?.Invoke(cellHandler);

    public void PieceDragging(PieceHandler piece, CellHandler downCell) => PieceDragged?.Invoke(piece, downCell);

    private void SetupServices()
    {
        boardService.Setup();
        pieceService.Setup();

        BoardLoaded?.Invoke();
    }
}
