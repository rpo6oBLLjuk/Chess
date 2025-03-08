using System.Drawing;
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
        foreach (CellHandler cellHandler in gameController.Cells.Array)
        {
            if (cellHandler.CurrentPieceHandler != null)
                gameController.DestroyPiece(cellHandler);
        }

        byte currentPiece;
        for (byte y = 0; y < gameController.BoardSize.y; y++)
        {
            for (byte x = 0; x < gameController.BoardSize.x; x++)
            {
                currentPiece = gameController.Pieces[y * gameController.BoardSize.x + x];
                if (PiecePacker.GetPieceType(currentPiece) != PieceType.None)
                {
                    Instantiate(currentPiece, gameController.Cells[x, y]);
                }
            }
        }
    }

    public GameObject Instantiate(byte pieceData, CellHandler cellHandler)
    {
        GameObject instance = container.InstantiatePrefab(pooler.Get(PiecePacker.GetPieceType(pieceData)), cellHandler.transform);

        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init(piecesSkinData.AnimationData);

        cellHandler.PiecePlaced(pieceHandler);

        gameController.Pieces[cellHandler.CellIndex] = pieceData;

        Debug.Log($"Piece ({PiecePacker.GetPieceColor(pieceData)}_{PiecePacker.GetPieceType(pieceData)}) instantiated", instance);
        return instance;
    }
}
