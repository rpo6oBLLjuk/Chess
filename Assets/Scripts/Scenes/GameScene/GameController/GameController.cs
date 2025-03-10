using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoService
{
    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPressedDown;

    public event Action<PieceHandler, CellHandler> PieceDragged;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    /// <summary>
    /// First arg: Eater, second arg: eated piece
    /// </summary>
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceCaptured;

    public event Action<PieceHandler, CellHandler> PieceSpawned;
    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    public event Action BoardLoaded;
    public event Action BoardCleared;

    [Header("Grids")]
    [field: SerializeField] public Grid<byte> Board { get; private set; }

    [field: SerializeField] public Grid<PieceHandler> Pieces { get; set; }
    [field: SerializeField] public Grid<CellHandler> Cells { get; set; }

    public GameData GameData => gameData;
    public PiecesSkinData PiecesSkinData => pieceService.piecesSkinData;
    public CellsSkinData CellsSkinData => boardService.cellsSkinData;

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

    public void SpawnPiece(byte pieceData, CellHandler cellHandler)
    {
        pieceService.SpawnPiece(pieceData, cellHandler);
        PieceSpawned.Invoke(Pieces[cellHandler.Index], cellHandler);
    }

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.CanBeMove(pieceHandler, startCell, endCell);

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        PieceMoved?.Invoke(pieceHandler, startCell, endCell);
    }
    public void CapturePiece(PieceHandler pieceHandler, CellHandler cellHandler)
    {
        pieceService.CapturePiece(cellHandler);
        PieceCaptured?.Invoke(pieceHandler, Pieces[cellHandler.Index], cellHandler);
    }

    public void DestroyPiece(CellHandler cellHandler)
    {
        pieceService.CapturePiece(cellHandler);
        PieceDestroyed?.Invoke(Pieces[cellHandler.Index], cellHandler);
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
