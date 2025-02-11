using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PieceBuilder
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private BoardBuilder boardCellBuilder;
    [SerializeField] private PieceFactory pooler = new();
    [SerializeField] private PiecesSkinData piecesSkinData;

    private PieceManager pieceManager;


    public void Init(PieceManager pieceManager)
    {
        this.pieceManager = pieceManager;
    }

    public void SetupPieces()
    {
        pieceManager.DeskData.PieceData = new PieceData[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];
        pieceManager.PieceInstances = new Transform[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];

        for (int y = 0; y < boardCellBuilder.BoardSize.y; y++)
        {
            for (int x = 0; x < boardCellBuilder.BoardSize.x; x++)
            {
                if (pieceManager.DeskData.PieceData[x, y] != null)
                {
                    Instantiate(pieceManager.DeskData.PieceData[x, y], boardManager.cells[x, y]);
                }
            }
        }
    }

    public GameObject Instantiate(PieceData pieceData, CellHandler boardCell)
    {
        GameObject instance = UnityEngine.Object.Instantiate(pooler.Get(pieceData.Type), boardCell.transform);
        instance.GetComponentInChildren<Image>().sprite = piecesSkinData.Get(pieceData);

        if (!instance.TryGetComponent(out PieceHandler pieceHandler))
            pieceHandler = instance.AddComponent<PieceHandler>();
        pieceHandler.PieceAnimationData = piecesSkinData.AnimationData;

        pieceManager.PieceInstances[boardCell.CellIndex.x, boardCell.CellIndex.y] = instance.transform;
        pieceManager.DeskData.PieceData[boardCell.CellIndex.x, boardCell.CellIndex.y] = pieceData;


        Debug.Log(instance, instance);
        return instance;
    }
}
