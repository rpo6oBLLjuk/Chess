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


    public void Init(CellsSkinData cellsSkinData, GridLayoutGroup boardGridLayout, GameObject cellPrefab)
    {
        this.cellsSkinData = cellsSkinData;
        this.boardGridLayout = boardGridLayout;
        this.cellPrefab = cellPrefab;
    }


    public void SetupBoard()
    {
        if (gameController.Cells != null)
            foreach (CellHandler cellHandler in gameController.Cells.Array)
            {
                if (cellHandler != null)
                    UnityEngine.Object.DestroyImmediate(cellHandler);
            }
        gameController.Cells = new(gameController.Board.Width, gameController.Board.Height);

        boardGridLayout.constraint = (gameController.Board.Width > gameController.Board.Height) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (gameController.Board.Width >= gameController.Board.Height) ? gameController.Board.Width : gameController.Board.Height;

        bool isWhite = leftUpCellIsWhite;

        for (byte y = 0; y < gameController.Board.Height; y++)
        {
            if (gameController.Board.Width % 2 == 0)
                isWhite = !isWhite;

            for (byte x = 0; x < gameController.Board.Width; x++)
            {
                isWhite = !isWhite;

                GameObject instance = container.InstantiatePrefab(cellPrefab, boardGridLayout.transform);
                instance.GetComponentInChildren<Image>().sprite = isWhite ? cellsSkinData.WhiteCell : cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponent<CellHandler>();

                gameController.Cells[x, y] = cellHandler;
                cellHandler.Init((byte)(y * gameController.Board.Width + x));
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
