using System.Linq;
using UnityEngine;
using Zenject;

public class PieceService : MonoService
{
    [Inject] GameController gameController;
    [Inject] NotificationService notificationService;

    PieceBuilder pieceBuilder;
    PieceMovementController pieceMovementController;
    PieceCapturer pieceCapturer;

    public PiecesSkinData piecesSkinData;
    [SerializeField] PiecePrefabs piecePrefabs;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMovementController = container.Instantiate<PieceMovementController>();
        pieceCapturer = container.Instantiate<PieceCapturer>();

        pieceBuilder.Init(piecesSkinData, piecePrefabs);
        pieceCapturer.Init();
    }

    public void Setup() => pieceBuilder.SetupPieces();

    public void ClearBoard() => gameController.Cells.Array.Where(cellHandler => gameController.Board[cellHandler.Index] != 0).ToList().ForEach(cellHandler => gameController.DestroyPiece(cellHandler));

    public void SpawnPiece(byte pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);

    public void CapturePiece(CellHandler cellHandler) => pieceCapturer.CapturePiece(cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = pieceMovementController.CanBeMove(startCell, endCell);
        if (!canMove)
            notificationService.ShowPopup("Move blocked", "Piece manager", PopupType.Warning);
        return canMove;
    }
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceMovementController.Move(pieceHandler, startCell, endCell);
}
