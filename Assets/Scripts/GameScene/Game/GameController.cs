using System;
using UnityEngine;
using Zenject;

public class GameController : MonoService
{
    [Inject] PieceService pieceService;
    [Inject] BoardService boardService;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceEaten;

    public event Action<PieceHandler, CellHandler> PieceDestroyed;


    public void MovePiece(PieceHandler piece, CellHandler startCell, PieceHandler endCell)
    {

    }
    public void EatPiece(PieceHandler eater, PieceHandler eatenPiece, CellHandler eatenPieceHandler)
    {

    }
    public void DestroyPiece(PieceHandler pieceHandler, CellHandler cellHandler)
    {

    }

}
