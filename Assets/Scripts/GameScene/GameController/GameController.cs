using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    [Inject] DeskSaverService deskSaver;
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;

    public string mainSaveName = "main";

    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPointerDown;

    public event Action<PieceHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler> PieceEaten;

    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    [field: SerializeField] public BoardPiecesData PiecesData { get; private set; }
    [field: SerializeField] public BoardCellsData CellsData { get; private set; }


    public void Setup()
    {
        pieceService.OnInstantiated();
        boardService.OnInstantiated();

        StartDataLoad();
    }

    public void SpawnPiece(PieceData pieceData, CellHandler cellHandler) => pieceService.SpawnPiece(pieceData, cellHandler);
    public void SpawnPiece(PieceType pieceType, PieceColor pieceColor, CellHandler cellHandler) => pieceService.SpawnPiece(pieceType, pieceColor, cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.CanBeMove(pieceHandler, startCell, endCell);

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        PieceMoved?.Invoke(pieceHandler);
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
        throw new System.Exception("Destroy-метод удалён при рефакторинге кода");
        //pieceService.DestroyPiece(pieceHandler);
    }

    public void ClickOnCell(CellHandler cellHandler)
    {
        CellClicked?.Invoke(cellHandler);
        Debug.Log("Piece clicked");
    }
    public void PointerDownOnCell(CellHandler cellHandler)
    {
        CellPointerDown?.Invoke(cellHandler);
        Debug.Log("Piece pointer down");
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
