using UnityEngine;
using Zenject;

public class PieceService : MonoService
{
    [Inject] NotificationService notificationService;

    private PieceBuilder pieceBuilder;
    private PieceMovementController pieceMovementController;

    [SerializeField] PieceSkinsData pieceSkinsData;
    [SerializeField] PiecePrefabs piecePrefabs;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMovementController = container.Instantiate<PieceMovementController>();

        pieceBuilder.Init(pieceSkinsData, piecePrefabs);
    }

    public void Setup()
    {
        pieceBuilder.SetupPieces();

        pieceMovementController.PieceMoved += PieceMoved;
    }

    public void SpawnPiece(PieceData pieceData, CellHandler cellHandler)
    {
        pieceBuilder.Instantiate(pieceData, cellHandler);
    }
    public void SpawnPiece(PieceType type, PieceColor color, CellHandler cellHandler)
    {
        SpawnPiece(new PieceData(type, color), cellHandler);
    }

    public bool CanBeMove(PieceHandler pieceHandler)
    {
        bool canMove = pieceMovementController.CanBeMove(pieceHandler);
        if (!canMove)
            notificationService.ShowPopup("Move blocked", "Piece manager", PopupType.Warning);
        return canMove;
    }
    public void MovePiece(PieceHandler pieceHandler) => pieceMovementController.Move(pieceHandler);

    protected void PieceMoved(PieceData pieceData, CellHandler startCellHandler, CellHandler endCellHandler)
    {
        DebugExtensions.Log($"Piece '{pieceData.Type}' move from {startCellHandler.CellIndex} to {endCellHandler.CellIndex}", "PieceService");
    }
}
