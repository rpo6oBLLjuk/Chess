using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BoardBuilder
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private GameObject whiteCell;
    [SerializeField] private GameObject blackCell;

    [Header("Board data")]
    [SerializeField] private bool leftUpCellIsWhite = true;

    private PieceService pieceService;
    private BoardService boardService;


    public void Init(BoardService boardService, PieceService pieceService)
    {
        this.boardService = boardService;
        this.pieceService = pieceService;
    }


    public void SetupBoard(DeskData deskData)
    {
        boardService.cells = new CellHandler[deskData.BoardSize.x, deskData.BoardSize.y]; //board Init

        boardGridLayout.constraint = (deskData.BoardSize.x > deskData.BoardSize.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        boardGridLayout.constraintCount = (deskData.BoardSize.x >= deskData.BoardSize.y) ? deskData.BoardSize.x : deskData.BoardSize.y;

        bool isWhite = !leftUpCellIsWhite;

        Debug.Log(deskData.BoardSize);
        for (int y = 0; y < deskData.BoardSize.y; y++)
        {
            if (deskData.BoardSize.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < deskData.BoardSize.x; x++)
            {
                GameObject cell = UnityEngine.Object.Instantiate(isWhite ? whiteCell : blackCell, boardGridLayout.transform);
                isWhite = !isWhite;

                if (!cell.TryGetComponent(out CellHandler cellHandler))
                    cellHandler = cell.AddComponent<CellHandler>();

                boardService.cells[x, y] = cellHandler;
                cellHandler.Init(x, y);

                AddCallbackListener(cellHandler);

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

    public void AddCallbackListener(CellHandler cellHandler)
    {
        cellHandler.OnPiecePlaced += CellCallback;
    }

    private void CellCallback(CellHandler cellHandler, PieceHandler pieceHandler)
    {
        pieceService.MovePiece(pieceHandler);
    }
}
