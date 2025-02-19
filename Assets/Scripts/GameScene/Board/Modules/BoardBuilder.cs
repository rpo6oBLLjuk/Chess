using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class BoardBuilder
{
    private GameController gameController;

    [Header("References")]
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private GameObject whiteCell;
    [SerializeField] private GameObject blackCell;

    [Header("Board data")]
    [SerializeField] private bool leftUpCellIsWhite = true;

    private PieceService pieceService;
    private BoardService boardService;


    public void Init(BoardService boardService, PieceService pieceService, GameController gameController)
    {
        this.gameController = gameController;
        this.boardService = boardService;
        this.pieceService = pieceService;
    }


    public void SetupBoard(BoardPiecesData deskData)
    {
        gameController.CellsData.SetSize(deskData.Size);

        boardGridLayout.constraint = (deskData.Size.x > deskData.Size.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (deskData.Size.x >= deskData.Size.y) ? deskData.Size.x : deskData.Size.y;

        bool isWhite = !leftUpCellIsWhite;

        for (int y = 0; y < deskData.Size.y; y++)
        {
            if (deskData.Size.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < deskData.Size.x; x++)
            {
                GameObject cell = UnityEngine.Object.Instantiate(isWhite ? whiteCell : blackCell, boardGridLayout.transform);
                isWhite = !isWhite;

                if (!cell.TryGetComponent(out CellHandler cellHandler))
                    cellHandler = cell.AddComponent<CellHandler>();

                gameController.CellsData.Set(x, y, cellHandler);
                cellHandler.Init(x, y);

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
