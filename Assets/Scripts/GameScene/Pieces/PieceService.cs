using UnityEngine;
using Zenject;

public class PieceService : MonoService
{
    [Inject] GameController gameController;
    [Inject] NotificationService notificationService;

    private PieceBuilder pieceBuilder;
    private PieceMovementController pieceMovementController;
    private PieceDestroyer pieceDestroyer;

    public PiecesSkinData piecesSkinData;
    [SerializeField] PiecePrefabs piecePrefabs;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        pieceBuilder = container.Instantiate<PieceBuilder>();
        pieceMovementController = container.Instantiate<PieceMovementController>();
        pieceDestroyer = container.Instantiate<PieceDestroyer>();

        pieceBuilder.Init(piecesSkinData, piecePrefabs);
    }

    public void Setup()
    {
        pieceBuilder.SetupPieces();

        gameController.PieceMoved += PieceMoved;
    }

    private void OnDisable()
    {
        gameController.PieceMoved -= PieceMoved;
    }

    public void SpawnPiece(PieceData pieceData, CellHandler cellHandler) => pieceBuilder.Instantiate(pieceData, cellHandler);
    public void SpawnPiece(PieceType type, PieceColor color, CellHandler cellHandler) => SpawnPiece(new PieceData(type, color), cellHandler);

    public void DestroyPiece(CellHandler cellHandler) => pieceDestroyer.DestroyPiece(cellHandler);

    public bool CanBeMove(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        bool canMove = pieceMovementController.CanBeMove(pieceHandler, startCell, endCell);
        if (!canMove)
            notificationService.ShowPopup("Move blocked", "Piece manager", PopupType.Warning);
        return canMove;
    }
    public void MovePiece(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => pieceMovementController.Move(pieceHandler, startCell, endCell);

    protected void PieceMoved(PieceHandler pieceHandler)
    {
        DebugExtensions.Log($"Piece '{pieceHandler.PieceData.Type}' move to {pieceHandler.CurrentCell.CellIndex}", "PieceService");
    }
}
