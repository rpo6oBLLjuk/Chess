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
    [Inject] SkinData skinData;

    private GridLayoutGroup boardGridLayout;
    private GameObject cellPrefab;

    private bool leftUpCellIsWhite = true;

    public void Init(GridLayoutGroup boardGridLayout, GameObject cellPrefab)
    {
        this.boardGridLayout = boardGridLayout;
        this.cellPrefab = cellPrefab;
    }


    public void SetupBoard()
    {
        if (gameManager.Cells.Count() != 0)
            return;

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
                instance.GetComponentInChildren<Image>().sprite = isWhite ? skinData.cellsSkinData.WhiteCell : skinData.cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponent<CellHandler>();

                int index = y * 8 + x;
                gameManager.Cells[index] = cellHandler;
                cellHandler.Init((byte)(y * 8 + x));
                cellHandler.CellEffectController.Init(skinData.cellsSkinData);
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
