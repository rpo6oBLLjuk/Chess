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
    [SerializeField] PieceMover pieceMover;
    [SerializeField] PieceCapturer pieceCapturer;

    [SerializeField] bool logging = false;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMover = container.Instantiate<PieceMover>();
        pieceCapturer = container.Instantiate<PieceCapturer>();

        MoveChecker = container.Instantiate<MoveChecker>();
        MovesGenerator = container.Instantiate<MovesGenerator>();

        pieceBuilder.Init(piecePrefabs);
        pieceCapturer.Init();

        MovesGenerator.Init(this);

        gameManager.GameDataChanged += GameDataChanged;
    }

    private void OnDisable()
    {
        gameManager.GameDataChanged -= GameDataChanged;
    }

    public void Setup()
    {
        pieceBuilder.SetupPieces();
        MovesGenerator.GenerateAllPossibleMoves(gameManager.GameTurnController.TurnColor);
    }

    public void ClearBoard() => gameManager.Cells.Where(cellHandler => !PiecePacker.IsEqualType(gameManager.Board[cellHandler.Index], PieceType.None)).ToList().ForEach(cellHandler => gameManager.DestroyPiece(cellHandler));

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);
    public void CapturePiece(CellHandler cellHandler) => pieceCapturer.CapturePiece(cellHandler);

    public bool IsMoveAllowed(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = MoveChecker.IsGameMoveAllowed(startCell.Index, endCell.Index);
        if (!canMove && logging)
            this.InactiveLog("Move blocked");
        return canMove;
    }

    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceMover.Move(pieceHandler, startCell, endCell);

    private void GameDataChanged() => MovesGenerator.GenerateAllPossibleMoves(gameManager.GameTurnController.TurnColor);
}
