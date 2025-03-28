using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoService
{
    public event Action GameDataChanged
    {
        add => GameData.DataChanged += value;
        remove => GameData.DataChanged -= value;
    }

    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPressedDown;
    /// <summary>
    /// Called if the movement is blocked and the shape is released.
    /// </summary>
    public event Action<CellHandler> CellPressedUp;

    public event Action<PieceHandler, CellHandler> PieceDragged;

    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    /// <summary>
    /// First arg: Capturer, second arg: captured piece
    /// </summary>
    public event Action<PieceHandler, PieceHandler, CellHandler> PieceCaptured;

    public event Action<PieceHandler, CellHandler> PieceSpawned;
    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    public event Action BoardLoaded;
    public event Action BoardCleared;

    public GameTurnController GameTurnController { get; private set; } = new();

    [Header("Arrays")]
    [field: SerializeField] public byte[] Board { get; set; }
    [field: SerializeField] public PieceHandler[] Pieces { get; set; }
    [field: SerializeField] public CellHandler[] Cells { get; set; }
    [field: SerializeField] public List<byte>[] Moves; //Field for ref-args

    [Header("Data")]
    public GameData GameData => gameData;
    public PiecesSkinData PiecesSkinData => pieceService.PiecesSkinData;
    public CellsSkinData CellsSkinData => boardService.CellsSkinData;

    [Header("Dependencies")]
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;

    [Header("Data")]
    [SerializeField] private GameData gameData;




    public void Setup()
    {
        pieceService.OnInstantiated();
        boardService.OnInstantiated();

        LoadDefaultBoard();

        SetupServices();
    }

    public void ClearBoard()
    {
        pieceService.ClearBoard();
        BoardCleared?.Invoke();
    }

    public void SetCustomBoard(byte[] boardPiecesData)
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
    public void CapturePiece(PieceHandler pieceHandler, CellHandler cellHandler)
    {
        PieceHandler capturedPiece = Pieces[cellHandler.Index];
        pieceService.CapturePiece(cellHandler);
        PieceCaptured?.Invoke(pieceHandler, capturedPiece, cellHandler);
    }
    public void DestroyPiece(CellHandler cellHandler)
    {
        PieceHandler capturedPiece = Pieces[cellHandler.Index];
        pieceService.CapturePiece(cellHandler);
        PieceDestroyed?.Invoke(capturedPiece, cellHandler);
    }

    public bool IsMoveAllowed(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.IsMoveAllowed(pieceHandler, startCell, endCell);
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);
        GameTurnController.PieceMoved();
        PieceMoved?.Invoke(pieceHandler, startCell, endCell);
    }

    public void ClickOnCell(CellHandler cellHandler) => CellClicked?.Invoke(cellHandler);
    public void PressDownOnCell(CellHandler cellHandler) => CellPressedDown?.Invoke(cellHandler);
    public void PressUpOnCell(CellHandler cellHandler) => CellPressedUp?.Invoke(cellHandler);

    public void PieceDragging(PieceHandler piece, CellHandler downCell) => PieceDragged?.Invoke(piece, downCell);


    private void SetupServices()
    {
        boardService.Setup();
        pieceService.Setup();

        BoardLoaded?.Invoke();
    }
    private void LoadDefaultBoard()
    {
        Board = new byte[64];

        #region Rooks
        Board[ArrayWrapper.ConvertCoordinateToIndex(0, 0)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);
        Board[ArrayWrapper.ConvertCoordinateToIndex(7, 0)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);

        Board[ArrayWrapper.ConvertCoordinateToIndex(0, 7)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        Board[ArrayWrapper.ConvertCoordinateToIndex(7, 7)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        #endregion

        #region Knights
        Board[ArrayWrapper.ConvertCoordinateToIndex(1, 0)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);
        Board[ArrayWrapper.ConvertCoordinateToIndex(6, 0)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);

        Board[ArrayWrapper.ConvertCoordinateToIndex(1, 7)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        Board[ArrayWrapper.ConvertCoordinateToIndex(6, 7)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        #endregion

        #region Bishops
        Board[ArrayWrapper.ConvertCoordinateToIndex(2, 0)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);
        Board[ArrayWrapper.ConvertCoordinateToIndex(5, 0)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);

        Board[ArrayWrapper.ConvertCoordinateToIndex(2, 7)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        Board[ArrayWrapper.ConvertCoordinateToIndex(5, 7)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        #endregion

        #region Queens
        Board[ArrayWrapper.ConvertCoordinateToIndex(3, 0)] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.Black);
        Board[ArrayWrapper.ConvertCoordinateToIndex(3, 7)] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.White);
        #endregion

        #region Kings
        Board[ArrayWrapper.ConvertCoordinateToIndex(4, 0)] = PiecePacker.PackPiece(PieceType.King, PieceColor.Black);
        Board[ArrayWrapper.ConvertCoordinateToIndex(4, 7)] = PiecePacker.PackPiece(PieceType.King, PieceColor.White);
        #endregion

        #region Pawns
        for (byte i = 0; i < 8; i++)
        {
            Board[ArrayWrapper.ConvertCoordinateToIndex(i, 1)] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.Black);
            Board[ArrayWrapper.ConvertCoordinateToIndex(i, 6)] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.White);
        }
        #endregion
    }
}
