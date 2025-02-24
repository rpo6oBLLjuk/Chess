using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    [Inject] DeskSaverService deskSaver;
    [Inject] PieceService pieceService;
    [Inject] BoardService boardService;

    public string mainSaveName = "main";

    public event Action<PieceHandler> PiecePointerClicked;
    public event Action<PieceHandler> PiecePointerDown;

    public event Action<PieceHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler> PieceEaten;

    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    [field: SerializeField] public BoardPiecesData PiecesData { get; private set; }
    [field: SerializeField] public BoardCellsData CellsData { get; private set; }


    public void Setup()
    {
        StartDataLoad();
    }

    public void MovePiece(PieceHandler pieceHandler)
    {
        pieceService.MovePiece(pieceHandler);
        PieceMoved(pieceHandler);
    }
    public void EatPiece(CellHandler eatenPieceHandler)
    {
        pieceService.DestroyPiece(eatenPieceHandler);
        Debug.Log("Piece eaten");
    }

    public void DestroyPiece(CellHandler cellHandler)
    {
        pieceService.DestroyPiece(cellHandler);
    }
    public void DestroyPiece(PieceHandler pieceHandler)
    {
        pieceService.DestroyPiece(pieceHandler.CurrentCell);
    }

    public void ClickOnPiece(PieceHandler pieceHandler)
    {
        PiecePointerClicked?.Invoke(pieceHandler);
        Debug.Log("Piece clicked");
    }
    public void DownOnPiece(PieceHandler pieceHandler)
    {
        PiecePointerDown?.Invoke(pieceHandler);
    }

    private void StartDataLoad()
    {
        PiecesData = deskSaver.LoadBoard(mainSaveName);

        if (PiecesData != null)
        {
            Debug.Log(PiecesData.Size);
        }
        else
        {
            PiecesData = new BoardPiecesData(8, 8);
        }

        Debug.Log($"First: {PiecesData.Data.First().Type}");

        CellsData = new(PiecesData.Size);

        boardService.Setup();
        pieceService.Setup();
    }
}
