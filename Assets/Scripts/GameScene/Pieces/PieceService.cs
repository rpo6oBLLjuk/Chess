using UnityEngine;
using Zenject;

public class PieceService : MonoService
{
    [Inject] BoardService boardService;

    private PieceBuilder pieceBuilder;
    private PieceMovementController pieceMovementController = new();

    [SerializeField] PieceSkinsData pieceSkinsData;
    [SerializeField] PiecePrefabs piecePrefabs;

    public Transform[,] PieceInstances { get; set; }

    public DeskData DeskData;


    public void Setup(DeskData deskData)
    {
        this.DeskData = deskData;

        pieceBuilder = container.Instantiate<PieceBuilder>();

        pieceBuilder.Init(this, boardService, pieceSkinsData, piecePrefabs);
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
