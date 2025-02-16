using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class PieceBuilder
{
    [Inject] DiContainer container;

    [Inject] GameController gameController;

    [Inject] BoardService boardService;
    [Inject] PieceService pieceService;

    private PieceFactory pooler = new();
    private PieceSkinsData piecesSkinData;


    public void Init(PieceSkinsData piecesSkinData, PiecePrefabs piecePrefabs)
    {
        this.piecesSkinData = piecesSkinData;
        pooler.PiecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        for (int y = 0; y < gameController.DeskData.BoardSize.y; y++)
        {
            for (int x = 0; x < gameController.DeskData.BoardSize.x; x++)
            {
                if (gameController.DeskData.BoardData[x, y] != null)
                {
                    Instantiate(gameController.DeskData.BoardData[x, y], boardService.cells[x, y]);
                }
            }
        }
    }

    public GameObject Instantiate(PieceData pieceData, CellHandler boardCell)
    {
        GameObject instance = container.InstantiatePrefab(pooler.Get(pieceData.Type), boardCell.transform);
        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.Init(piecesSkinData.AnimationData, pieceData);

        //RectTransform rectTransform = instance.GetComponent<RectTransform>();
        //rectTransform.anchorMin = Vector2.zero;
        //rectTransform.anchorMax = Vector2.one;
        //rectTransform.offsetMin = Vector2.zero;
        //rectTransform.offsetMax = Vector2.zero;

        gameController.DeskData.BoardData[boardCell.CellIndex.x, boardCell.CellIndex.y] = pieceData;

        Debug.Log(instance, instance);
        return instance;
    }
}
