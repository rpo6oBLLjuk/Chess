using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoService
{
    public event Action<PieceColor, bool> GameEnded;

    public Action<PieceHandler> OnKingChecked;
    public Action<PieceHandler> OnCheckResolved;

    public event Action<CellHandler> CellClicked;
    public event Action<CellHandler> CellPressedDown;

    public event Action<PieceHandler> PieceDragStarted;
    public event Action<PieceHandler, Vector3, CellHandler> PieceDragged;
    public event Action<PieceHandler, CellHandler> PieceDragEnded;

    /// <summary>
    /// Args: Moved Piece, from cell, to cell
    /// </summary>
    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoved;
    /// <summary>
    /// Args: Moved Piece, from cell, to cell
    /// </summary>
    public event Action<PieceHandler, CellHandler, CellHandler> PieceMoveBlocked;
    /// <summary>
    /// Args: Capturer, Captured piece, Captured piece data, End cell 
    /// </summary>
    public event Action<PieceHandler, PieceHandler, byte, CellHandler> PieceCaptured;

    public event Action<PieceHandler, CellHandler> PieceSpawned;
    public event Action<PieceHandler, CellHandler> PieceDestroyed;

    public event Action BoardLoaded;
    public event Action BoardCleared;

    public GameTurnController GameTurnController { get; private set; } = new();

    [Header("Arrays")]
    [field: SerializeField] public byte[] Board { get; set; }
    [field: SerializeField] public PieceHandler[] Pieces { get; set; }
    [field: SerializeField] public CellHandler[] Cells { get; set; }

    [Space]
    [field: SerializeField] public List<byte>[] PossibleMoves; //Field for ref-args

    [field: Space, SerializeField] public List<Move> Moves { get; set; }
    [field: SerializeField] public List<Capture> Captures { get; set; }

    [Header("Data")]
    public GameData GameData => gameData;

    [Header("Dependencies")]
    [SerializeField] private PieceService pieceService;
    [SerializeField] private BoardService boardService;

    [Header("Override Data")]
    [SerializeField] private GameData gameData;

    [Inject] private GameData injectableGameData;


    public void Setup()
    {
        pieceService.Initialize();
        boardService.Initialize();

        gameData ??= injectableGameData;

        Board = new byte[64];

        if (GameData.LoadableBoard != null)
            Board = GameData.GetBoard();
        else
            Board = GameData.GetDefaultBoard();

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

    public void SpawnPiece(byte pieceData, CellHandler cellHandler, bool onBuild = false)
    {
        pieceService.SpawnPiece(pieceData, cellHandler);

        if (!onBuild)
            pieceService.MovesGenerator.GenerateAllPossibleMoves(GameTurnController.TurnColor);

        PieceSpawned.Invoke(Pieces[cellHandler.Index], cellHandler);
    }
    public void CapturePiece(PieceHandler capturer, CellHandler endCellHandler)
    {
        byte capturedPieceData = Board[endCellHandler.Index];
        PieceHandler capturedPiece = Pieces[endCellHandler.Index];

        pieceService.CapturePiece(capturer, endCellHandler);
        PieceCaptured?.Invoke(capturer, capturedPiece, capturedPieceData, endCellHandler);
    }
    public void DestroyPiece(CellHandler cellHandler)
    {
        PieceHandler capturedPiece = Pieces[cellHandler.Index];
        pieceService.DestroyPiece(cellHandler);

        pieceService.MovesGenerator.GenerateAllPossibleMoves(GameTurnController.TurnColor);

        PieceDestroyed?.Invoke(capturedPiece, cellHandler);
    }

    public bool IsMoveAllowed(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceService.IsMoveAllowed(pieceHandler, startCell, endCell);
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceService.MovePiece(pieceHandler, startCell, endCell);

        GameTurnController.PieceMoved();
        pieceService.MovesGenerator.GenerateAllPossibleMoves(GameTurnController.TurnColor);

        PieceMoved?.Invoke(pieceHandler, startCell, endCell);
    }
    public void BlockPieceMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        PieceMoveBlocked?.Invoke(pieceHandler, startCell, endCell);
    }

    public void ClickOnCell(CellHandler cellHandler) => CellClicked?.Invoke(cellHandler);
    public void PressDownOnCell(CellHandler cellHandler) => CellPressedDown?.Invoke(cellHandler);

    public void PieceStartDrag(PieceHandler piece) => PieceDragStarted?.Invoke(piece);
    public void PieceDragging(PieceHandler piece, Vector3 position, CellHandler downCell) => PieceDragged?.Invoke(piece, position, downCell);
    public void PieceEndDrag(PieceHandler piece, CellHandler endCell) => PieceDragEnded?.Invoke(piece, endCell);

    public void GameEnd(PieceColor pieceColor, bool pat) => GameEnded?.Invoke(pieceColor, pat);


    private void OnEnable() => injectableGameData.BoardChanged += SetCustomBoard;
    private void OnDisable() => injectableGameData.BoardChanged -= SetCustomBoard;

    private void SetupServices()
    {
        boardService.Setup();
        pieceService.Setup();

        BoardLoaded?.Invoke();
    }
    
}

[Serializable]
public struct Move
{
    public byte Piece;
    public byte StartIndex;
    public byte EndIndex;

    public Move(byte piece, byte startIndex, byte endIndex)
    {
        Piece = piece;
        StartIndex = startIndex;
        EndIndex = endIndex;
    }
}

[Serializable]
public struct Capture
{
    public byte MoveIndex;
    public byte CapturerPiece;
    public byte CapturedPiece;

    public Capture(byte moveIndex, byte capturerPiece, byte capturedPiece)
    {
        MoveIndex = moveIndex;
        CapturerPiece = capturerPiece;
        CapturedPiece = capturedPiece;
    }
}


