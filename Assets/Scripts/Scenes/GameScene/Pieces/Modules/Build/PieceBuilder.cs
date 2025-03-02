using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceBuilder
{
    [Inject] DiContainer container;
    [Inject] GameController gameController;

    PieceFactory pooler = new();
    PiecesSkinData piecesSkinData;


    public void Init(PiecesSkinData piecesSkinData, PiecePrefabs piecePrefabs)
    {
        this.piecesSkinData = piecesSkinData;
        pooler.PiecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        gameController.CellsData.Data.ForEach(cellHandler =>
        {
            if (cellHandler.CurrentPieceHandler != null)
                gameController.DestroyPiece(cellHandler);
        });

        PieceData currentPiece;
        for (int y = 0; y < gameController.PiecesData.Size.y; y++)
        {
            for (int x = 0; x < gameController.PiecesData.Size.x; x++)
            {
                currentPiece = gameController.PiecesData.GetPiece(x, y);
                if (currentPiece.Type != PieceType.None)
                {
                    Instantiate(currentPiece, gameController.CellsData.Get(x, y));
                }
            }
        }
    }

    public GameObject Instantiate(PieceData pieceData, CellHandler cellHandler)
    {
        GameObject instance = container.InstantiatePrefab(pooler.Get(pieceData.Type), cellHandler.transform);

        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init(piecesSkinData.AnimationData, pieceData, cellHandler);

        cellHandler.PiecePlaced(pieceHandler);

        gameController.PiecesData.SetPiece(cellHandler.CellIndex, pieceData);

        Debug.Log($"Piece ({pieceData.Color}_{pieceData.Type}) instantiated", instance);
        return instance;
    }
}
