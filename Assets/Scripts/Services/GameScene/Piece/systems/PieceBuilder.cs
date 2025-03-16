using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceBuilder
{
    [Inject] DiContainer container;
    [Inject] GameManager gameManager;

    PiecePrefabs piecesPrefabs;


    public void Init(PiecePrefabs piecePrefabs)
    {
        this.piecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        gameManager.Pieces = new PieceHandler[64];

        byte currentPiece;
        for (byte index = 0; index < 64; index++)
        {
            currentPiece = gameManager.Board[index];
            if (!PiecePacker.IsEqualType(ref currentPiece, PieceType.None))
            {
                gameManager.SpawnPiece(currentPiece, gameManager.Cells[index]);
            }
        }
    }

    public GameObject Instantiate(byte pieceData, CellHandler cellHandler)
    {
        GameObject instance = container.InstantiatePrefab(piecesPrefabs.Piece, cellHandler.transform);
        instance.name = $"Piece ({PiecePacker.GetFormattedData(ref pieceData)})]";
        instance.GetComponentInChildren<Image>().sprite = gameManager.PiecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init();

        cellHandler.PiecePlaced(pieceHandler);

        gameManager.Board[cellHandler.Index] = pieceData;
        gameManager.Pieces[cellHandler.Index] = pieceHandler;

        this.Log($"Piece {PiecePacker.GetFormattedDataWithColor(ref pieceData)} instantiated on cell {cellHandler.Index}", context: instance);
        return instance;
    }
}
