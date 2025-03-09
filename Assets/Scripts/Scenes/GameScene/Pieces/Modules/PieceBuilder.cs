using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceBuilder
{
    [Inject] DiContainer container;
    [Inject] GameController gameController;

    PiecePrefabs piecesPrefabs;
    PiecesSkinData piecesSkinData;




    public void Init(PiecesSkinData piecesSkinData, PiecePrefabs piecePrefabs)
    {
        this.piecesSkinData = piecesSkinData;
        this.piecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        foreach (CellHandler cellHandler in gameController.Cells.Array)
        {
            if (gameController.Board[cellHandler.Index] != 0)
                gameController.DestroyPiece(cellHandler);
        }
        gameController.Pieces = new(gameController.Board.Width, gameController.Board.Height);

        byte currentPiece;
        for (byte y = 0; y < gameController.Board.Height; y++)
        {
            for (byte x = 0; x < gameController.Board.Width; x++)
            {
                currentPiece = gameController.Board[y * gameController.Board.Width + x];
                if (PiecePacker.GetType(currentPiece) != PieceType.None)
                {
                    Instantiate(currentPiece, gameController.Cells[x, y]);
                }
            }
        }
    }

    public GameObject Instantiate(byte pieceData, CellHandler cellHandler)
    {
        GameObject instance = container.InstantiatePrefab(piecesPrefabs.Get(PiecePacker.GetType(pieceData)), cellHandler.transform);

        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init(piecesSkinData.AnimationData);

        cellHandler.PiecePlaced(pieceHandler);

        gameController.Board[cellHandler.Index] = pieceData;
        gameController.Pieces[cellHandler.Index] = pieceHandler;

        this.Log($"Piece {PiecePacker.GetFormattedData(pieceData)} instantiated on cell {cellHandler.Index}", context: instance);
        return instance;
    }
}
