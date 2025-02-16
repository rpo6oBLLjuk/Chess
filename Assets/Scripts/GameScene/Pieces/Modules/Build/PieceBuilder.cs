using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class PieceBuilder
{
    [Inject] DiContainer container;

    private PieceFactory pooler = new();
    private PieceSkinsData piecesSkinData;

    private BoardService boardService;
    private PieceService pieceService;


    public void Init(PieceService pieceService, BoardService boardService, PieceSkinsData piecesSkinData, PiecePrefabs piecePrefabs)
    {
        this.pieceService = pieceService;
        this.boardService = boardService;
        this.piecesSkinData = piecesSkinData;
        pooler.PiecesPrefabs = piecePrefabs;
    }

    public void SetupPieces()
    {
        pieceService.DeskData.PieceData = new PieceData[boardService.BoardSize.x, boardService.BoardSize.y];
        pieceService.PieceInstances = new Transform[boardService.BoardSize.x, boardService.BoardSize.y];

        for (int y = 0; y < boardService.BoardSize.y; y++)
        {
            for (int x = 0; x < boardService.BoardSize.x; x++)
            {
                if (pieceService.DeskData.PieceData[x, y] != null)
                {
                    Instantiate(pieceService.DeskData.PieceData[x, y], boardService.cells[x, y]);
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
        pieceHandler.Init(piecesSkinData.AnimationData);

        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;


        pieceService.PieceInstances[boardCell.CellIndex.x, boardCell.CellIndex.y] = instance.transform;
        pieceService.DeskData.PieceData[boardCell.CellIndex.x, boardCell.CellIndex.y] = pieceData;


        Debug.Log(instance, instance);
        return instance;
    }
}
