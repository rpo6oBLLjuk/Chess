using System.Linq;
using UnityEngine;
using Zenject;

public class PieceService : MonoService
{
    [Inject] GameManager gameManager;
    [Inject] NotificationService notificationService;

    [field: Header("Data"), SerializeField]
    public PiecesSkinData PiecesSkinData { get; private set; }

    [SerializeField] PiecePrefabs piecePrefabs;

    public MoveChecker MoveChecker { get; private set; }

    PieceBuilder pieceBuilder;
    PieceMover pieceMover;
    PieceCapturer pieceCapturer;
    MovesGenerator movesGenerator;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMover = container.Instantiate<PieceMover>();
        pieceCapturer = container.Instantiate<PieceCapturer>();

        MoveChecker = container.Instantiate<MoveChecker>();
        movesGenerator = container.Instantiate<MovesGenerator>();

        pieceBuilder.Init(piecePrefabs);
        pieceCapturer.Init();

        movesGenerator.Init(this);

        gameManager.GameDataChanged += GameDataChanged;
    }

    private void OnDisable()
    {
        gameManager.GameDataChanged -= GameDataChanged;
    }

    public void Setup()
    {
        pieceBuilder.SetupPieces();
        movesGenerator.GenerateAllPossibleMoves();
    }

    public void ClearBoard() => gameManager.Cells.Where(cellHandler => !PiecePacker.IsEqualType( gameManager.Board[cellHandler.Index], PieceType.None)).ToList().ForEach(cellHandler => gameManager.DestroyPiece(cellHandler));

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);

    public void CapturePiece(CellHandler cellHandler) => pieceCapturer.CapturePiece(cellHandler);

    public bool IsMoveAllowed(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = MoveChecker.IsMoveAllowed(startCell.Index, endCell.Index);
        if (!canMove)
            notificationService.ShowPopup("Move blocked", "Piece manager", PopupType.Warning);
        return canMove;
    }
    
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        pieceMover.Move(pieceHandler, startCell, endCell);
        movesGenerator.GenerateAllPossibleMoves();
    }

    private void GameDataChanged()
    {
        movesGenerator.GenerateAllPossibleMoves();
    }
}
