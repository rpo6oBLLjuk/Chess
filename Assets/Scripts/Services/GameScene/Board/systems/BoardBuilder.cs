using DG.Tweening.Core.Easing;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class BoardBuilder
{
    [Inject] DiContainer container;
    [Inject] GameManager gameManager;

    private GridLayoutGroup boardGridLayout;
    private GameObject cellPrefab;

    private CellsSkinData cellsSkinData;

    private bool leftUpCellIsWhite = true;


    public void Init(CellsSkinData cellsSkinData, GridLayoutGroup boardGridLayout, GameObject cellPrefab)
    {
        this.cellsSkinData = cellsSkinData;
        this.boardGridLayout = boardGridLayout;
        this.cellPrefab = cellPrefab;
    }


    public void SetupBoard()
    {
        gameManager.Cells?.Where(cellHandler => cellHandler != null).ToList().ForEach(cellHandler => UnityEngine.Object.Destroy(cellHandler.gameObject));
        gameManager.Cells = new CellHandler[64];

        boardGridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = 8;

        bool isWhite = leftUpCellIsWhite;

        for (byte y = 0; y < 8; y++)
        {
            isWhite = !isWhite;

            for (byte x = 0; x < 8; x++)
            {
                isWhite = !isWhite;

                GameObject instance = container.InstantiatePrefab(cellPrefab, boardGridLayout.transform);
                instance.name = $"Cell [{x},{y}]";
                instance.GetComponentInChildren<Image>().sprite = isWhite ? cellsSkinData.WhiteCell : cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponent<CellHandler>();

                int index = y * 8 + x;
                gameManager.Cells[index] = cellHandler;
                cellHandler.Init((byte)(y * 8 + x));
                cellHandler.CellEffectController.Init(cellsSkinData);
            }
        }

        if (!boardGridLayout.gameObject.TryGetComponent(out ContentSizeFitter fitter))
        {
            fitter = boardGridLayout.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }
}
