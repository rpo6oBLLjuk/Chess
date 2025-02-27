using System;
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
        BoardPiecesData deskData = gameController.PiecesData;
        gameController.CellsData.SetSize(deskData.Size);

        boardGridLayout.constraint = (deskData.Size.x > deskData.Size.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (deskData.Size.x >= deskData.Size.y) ? deskData.Size.x : deskData.Size.y;

        bool isWhite = leftUpCellIsWhite;

        for (int y = 0; y < deskData.Size.y; y++)
        {
            if (deskData.Size.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < deskData.Size.x; x++)
            {
                isWhite = !isWhite;

                GameObject instance = container.InstantiatePrefab(cellPrefab, boardGridLayout.transform);
                instance.GetComponentInChildren<Image>().sprite = isWhite ? cellsSkinData.WhiteCell : cellsSkinData.BlackCell;

                CellHandler cellHandler = instance.GetComponentInChildren<CellHandler>();

                gameController.CellsData.Set(x, y, cellHandler);
                cellHandler.Init(x, y);
                cellHandler.CellEffectController.Init(cellsSkinData);

                //AddCallbackListener(cellHandler);

                //float r = Random.Range(0f, 1f);
                //float g = Random.Range(0f, 1f);
                //float b = Random.Range(0f, 1f);

                //cell.GetComponentInChildren<CellTargetController>().SetAnimColor(new Color(r, g, b), 2f);
            }
        }

        ContentSizeFitter fitter = boardGridLayout.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    //public void AddCallbackListener(CellHandler cellHandler)
    //{
    //    cellHandler.OnPiecePlaced += CellCallback;
    //}

    //private void CellCallback(CellHandler cellHandler, PieceHandler pieceHandler)
    //{
    //    pieceService.MovePiece(pieceHandler);
    //}
}
