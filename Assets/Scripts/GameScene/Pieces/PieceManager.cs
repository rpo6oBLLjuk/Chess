using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private PieceBuilder pieceBuilder;
    [SerializeField] private PieceMovementController pieceMovementController;

    public Transform[,] PieceInstances { get; set; }

    public DeskData DeskData;


    public void Setup(DeskData deskData)
    {
        this.DeskData = deskData;

        pieceBuilder.Init(this);
        pieceMovementController.Init(this);

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

    public void MovePiece(PieceHandler pieceHandler, CellHandler endCellHandler)
    {
        pieceMovementController.Move(pieceHandler, endCellHandler);
    }

    public void PieceMoved(PieceData pieceData, CellHandler startCellHandler, CellHandler endCellHandler)
    {
        Debug.Log($"Piece {pieceData.Type} move from {startCellHandler.CellIndex} to {endCellHandler.CellIndex}");
    }
}
