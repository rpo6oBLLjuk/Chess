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

    [SerializeField] private List<GameObject> cells = new();

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

        gameController.Cells = new(gameController.Pieces.Width, gameController.Pieces.Height);

        boardGridLayout.constraint = (gameController.Pieces.Width > gameController.Pieces.Height) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (gameController.Pieces.Width >= gameController.Pieces.Height) ? gameController.Pieces.Width : gameController.Pieces.Height;

        bool isWhite = leftUpCellIsWhite;

        for (byte y = 0; y < gameController.Pieces.Height; y++)
        {
            if (gameController.Pieces.Width % 2 == 0)
                isWhite = !isWhite;

            for (byte x = 0; x < gameController.Pieces.Width; x++)
            {
                isWhite = !isWhite;

                GameObject instance = container.InstantiatePrefab(cellPrefab, boardGridLayout.transform);
                cells.Add(instance);

                instance.GetComponentInChildren<Image>().sprite = isWhite ? cellsSkinData.WhiteCell : cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponent<CellHandler>();

                gameController.Cells[x, y] = cellHandler;
                cellHandler.Init((byte)(y * gameController.Pieces.Width + x));
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
