using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class BoardBuilder
{
    [Inject] DiContainer container;
    [Inject] private GameController gameController;

    private GridLayoutGroup boardGridLayout;
    private GameObject cellPrefab;

    private CellsSkinData cellsSkinData;

    private bool leftUpCellIsWhite = true;

    [SerializeField]private List<GameObject> cells = new();

    public void Init(CellsSkinData cellsSkinData, GridLayoutGroup boardGridLayout, GameObject cellPrefab)
    {
        this.cellsSkinData = cellsSkinData;
        this.boardGridLayout = boardGridLayout;
        this.cellPrefab = cellPrefab;
    }


    public void SetupBoard()
    {
        cells.ForEach(instance => UnityEngine.Object.DestroyImmediate(instance));
        cells.Clear();

        BoardPiecesData piecesData = gameController.PiecesData;
        gameController.CellsData.SetSize(piecesData.Size);

        boardGridLayout.constraint = (piecesData.Size.x > piecesData.Size.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (piecesData.Size.x >= piecesData.Size.y) ? piecesData.Size.x : piecesData.Size.y;

        bool isWhite = leftUpCellIsWhite;

        for (int y = 0; y < piecesData.Size.y; y++)
        {
            if (piecesData.Size.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < piecesData.Size.x; x++)
            {
                isWhite = !isWhite;

                GameObject instance = container.InstantiatePrefab(cellPrefab, boardGridLayout.transform);
                cells.Add(instance);

                instance.GetComponentInChildren<Image>().sprite = isWhite ? cellsSkinData.WhiteCell : cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponentInChildren<CellHandler>();

                gameController.CellsData.Set(x, y, cellHandler);
                cellHandler.Init(x, y);
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
