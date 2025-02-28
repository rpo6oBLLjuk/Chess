using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceBuilder
{
    [Inject] DiContainer container;
    [Inject] GameController gameController;

    private PieceFactory pooler = new();
    private PiecesSkinData piecesSkinData;


    public void Init(PiecesSkinData piecesSkinData, PiecePrefabs piecePrefabs)
    {
        this.piecesSkinData = piecesSkinData;
        pooler.PiecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        PieceData currentPiece;
        for (int y = 0; y < gameController.PiecesData.Size.y; y++)
        {
            for (int x = 0; x < gameController.PiecesData.Size.x; x++)
            {
                currentPiece = gameController.PiecesData.Get(x, y);
                if (currentPiece != null)
                {
                    Instantiate(currentPiece, gameController.CellsData.Get(x, y));
                }
            }
        }
    }

    public GameObject Instantiate(PieceData pieceData, CellHandler cellHandler)
    {
        if (pieceData.Type == PieceType.None)
            return null;

        GameObject instance = container.InstantiatePrefab(pooler.Get(pieceData.Type), cellHandler.transform);
        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init(piecesSkinData.AnimationData, pieceData, cellHandler);

        cellHandler.PiecePlaced(pieceHandler);

        gameController.PiecesData.Set(cellHandler.CellIndex, pieceData);

        Debug.Log(instance, instance);
        return instance;
    }
}
