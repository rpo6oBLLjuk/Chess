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

    PieceBuilder pieceBuilder;
    PieceMover pieceMover;
    PieceCapturer pieceCapturer;
    MoveChecker moveChecker;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMover = container.Instantiate<PieceMover>();
        pieceCapturer = container.Instantiate<PieceCapturer>();

        moveChecker = container.Instantiate<MoveChecker>();

        pieceBuilder.Init(piecePrefabs);
        pieceCapturer.Init();
    }

    public void Setup() => pieceBuilder.SetupPieces();

    public void ClearBoard() => gameManager.Cells.Where(cellHandler => !PiecePacker.IsEqualType(ref gameManager.Board[cellHandler.Index], PieceType.None)).ToList().ForEach(cellHandler => gameManager.DestroyPiece(cellHandler));

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);

    public void CapturePiece(CellHandler cellHandler) => pieceCapturer.CapturePiece(cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = moveChecker.CanBeMove(startCell, endCell);
        if (!canMove)
            notificationService.ShowPopup("Move blocked", "Piece manager", PopupType.Warning);
        return canMove;
    }
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceMover.Move(pieceHandler, startCell, endCell);
}
