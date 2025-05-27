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
    public MovesGenerator MovesGenerator { get; private set; }

    [Header("Logging")]
    [SerializeField] PieceBuilder pieceBuilder;
    [SerializeField] PieceDataMover pieceDataMover;
    [SerializeField] PieceCapturer pieceCapturer;

    [SerializeField] PieceMover pieceMover;

    [SerializeField] PieceEffectManagerData data;
    [SerializeField] PieceEffectManager pieceEffectManager;

    [SerializeField] bool logging = false;


    public override void Initialize()
    {
        base.Initialize();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceBuilder.Init(piecePrefabs);

        pieceDataMover = container.Instantiate<PieceDataMover>();
        pieceCapturer = container.Instantiate<PieceCapturer>();
        pieceCapturer.Init();

        pieceMover = container.Instantiate<PieceMover>();
        pieceMover.Init();

        MoveChecker = container.Instantiate<MoveChecker>();

        MovesGenerator = container.Instantiate<MovesGenerator>();
        MovesGenerator.Init(this);

        pieceEffectManager = container.Instantiate<PieceEffectManager>();
        pieceEffectManager.Init(data);
    }

    private void OnDisable()
    {
        pieceMover.OnDisable();
        pieceEffectManager.OnDisable();
    }

    public void Setup()
    {
        pieceBuilder.SetupPieces();
        MovesGenerator.GenerateAllPossibleMoves(gameManager.GameTurnController.TurnColor);
    }

    public void ClearBoard() => gameManager.Cells.Where(cellHandler => !PiecePacker.IsEqualType(gameManager.Board[cellHandler.Index], PieceType.None)).ToList().ForEach(cellHandler => gameManager.DestroyPiece(cellHandler));

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);
    public void CapturePiece(PieceHandler capturer, CellHandler cellHandler) => pieceCapturer.CapturePiece(capturer, cellHandler);
    public void DestroyPiece(CellHandler cellHandler) => pieceCapturer.DestroyPiece(cellHandler);

    public bool IsMoveAllowed(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = MoveChecker.IsGameMoveAllowed(startCell.Index, endCell.Index);
        if (!canMove && logging)
            this.InactiveLog("Move blocked");
        return canMove;
    }

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceDataMover.Move(pieceHandler, startCell, endCell);
}
