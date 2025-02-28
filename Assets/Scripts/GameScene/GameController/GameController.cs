using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPointerDown;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceCaptured;
    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    [field: SerializeField] public BoardPiecesData PiecesData { get; private set; }
    [field: SerializeField] public BoardCellsData CellsData { get; private set; }

    [Inject] DeskSaverService deskSaver;
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;

    [SerializeField] private string mainSaveName = "main";


    public void Setup()
    {
        pieceService.OnInstantiated();
        boardService.OnInstantiated();

        LoadBoard();
    }

    public void ResetBoard() => LoadBoard();

    public void SpawnPiece(PieceData pieceData, CellHandler cellHandler) => pieceService.SpawnPiece(pieceData, cellHandler);
    public void SpawnPiece(PieceType pieceType, PieceColor pieceColor, CellHandler cellHandler) => pieceService.SpawnPiece(pieceType, pieceColor, cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.CanBeMove(pieceHandler, startCell, endCell);

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        PieceMoved?.Invoke(pieceHandler, startCell, endCell);

        Debug.Log($"Piece moved from {startCell.CellIndex} to {endCell.CellIndex}");
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
    public void PointerDownOnCell(CellHandler cellHandler)
    {
        CellPointerDown?.Invoke(cellHandler);

        Debug.Log($"Cell {cellHandler.CellIndex} pressed down ");
    }

    private void LoadBoard()
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
