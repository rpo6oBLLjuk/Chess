using System;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    [Inject] DeskSaverService deskSaver;
    [Inject] NotificationService notificationService;
    [Inject] PieceService pieceService;
    [Inject] BoardService boardService;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceEaten;

    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    public DeskData DeskData { get; private set; }


    public void Setup()
    {
        StartDataLoad();
    }

    public void MovePiece(PieceHandler piece, CellHandler startCell, PieceHandler endCell)
    {

    }
    public void EatPiece(PieceHandler eater, PieceHandler eatenPiece, CellHandler eatenPieceHandler)
    {

    }
    public void DestroyPiece(PieceHandler pieceHandler, CellHandler cellHandler)
    {

    }

    private void StartDataLoad()
    {
        DeskData = deskSaver.LoadBoard("main");

        if (DeskData != null)
        {
            Debug.Log(DeskData.BoardSize);
        }
        else
        {
            DeskData = new DeskData();
        }

        boardService.Setup();
        pieceService.Setup();
    }
}
