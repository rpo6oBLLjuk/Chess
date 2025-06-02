using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class PieceBuilder
{
    [Inject] DiContainer container;
    [Inject] GameManager gameManager;
    [Inject] SkinData skinData;

    PiecePrefabs piecesPrefabs;

    [SerializeField] bool logging = false;


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
            if (!PiecePacker.IsEqualType(currentPiece, PieceType.None))
            {
                gameManager.SpawnPiece(currentPiece, gameManager.Cells[index], true);
            }
        }
    }

    public GameObject Instantiate(byte pieceData, CellHandler cellHandler)
    {
        GameObject instance = container.InstantiatePrefab(piecesPrefabs.Piece, cellHandler.transform);
        instance.name = $"Piece ({PiecePacker.GetFormattedData(pieceData)})";
        instance.GetComponentInChildren<Image>().sprite = skinData.piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init();

        cellHandler.PiecePlaced(pieceHandler);

        gameManager.Board[cellHandler.Index] = pieceData;
        gameManager.Pieces[cellHandler.Index] = pieceHandler;

        if (logging)
            this.Log($"Piece {PiecePacker.GetFormattedDataWithColor(pieceData)} instantiated on cell {cellHandler.Index}", context: instance);
        
        return instance;
    }
}
