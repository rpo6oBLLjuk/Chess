using System;
using UnityEngine;

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

        LoadDefaultBoard();

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

    public void ClickOnCell(CellHandler cellHandler) => CellClicked?.Invoke(cellHandler);
    public void PressDownOnCell(CellHandler cellHandler) => CellPressedDown?.Invoke(cellHandler);

    public void PieceDragging(PieceHandler piece, CellHandler downCell) => PieceDragged?.Invoke(piece, downCell);

    private void SetupServices()
    {
        boardService.Setup();
        pieceService.Setup();

        BoardLoaded?.Invoke();
    }

    private void LoadDefaultBoard()
    {
        Board = new(8, 8);

        #region Rooks
        Board[0, 0] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);
        Board[7, 0] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);

        Board[0, 7] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        Board[7, 7] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        #endregion

        #region Knights
        Board[1, 0] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);
        Board[6, 0] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);

        Board[1, 7] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        Board[6, 7] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        #endregion

        #region Bishops
        Board[2, 0] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);
        Board[5, 0] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);

        Board[2, 7] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        Board[5, 7] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        #endregion

        #region Queens
        Board[3, 0] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.Black);
        Board[3, 7] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.White);
        #endregion

        #region Kings
        Board[4, 0] = PiecePacker.PackPiece(PieceType.King, PieceColor.Black);
        Board[4, 7] = PiecePacker.PackPiece(PieceType.King, PieceColor.White);
        #endregion

        #region Pawns
        for (int i = 0; i < 8; i++)
        {
            Board[i, 1] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.Black);
            Board[i, 6] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.White);
        }
        #endregion
    }
}
