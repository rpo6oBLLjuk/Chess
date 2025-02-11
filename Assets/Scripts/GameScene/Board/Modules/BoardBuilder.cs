using UnityEngine;
using UnityEngine.UI;

public class BoardBuilder : MonoBehaviour
{
    public Vector2Int BoardSize { get; private set; }

    [Header("References")]
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private GameObject whiteCell;
    [SerializeField] private GameObject blackCell;

    [Header("Board data")]
    [SerializeField] private bool leftUpCellIsWhite = true;

    private PieceManager pieceManager;
    private BoardManager boardManager;


    public void Init(BoardManager boardManager, PieceManager pieceManager)
    {
        this.boardManager = boardManager;
        this.pieceManager = pieceManager;
    }


    public void SetupBoard(DeskData deskData)
    {
        BoardSize = deskData.boardSize;

        boardManager.cells = new CellHandler[deskData.boardSize.x, deskData.boardSize.y]; //board Init

        boardGridLayout.constraint = (deskData.boardSize.x > deskData.boardSize.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;

        boardGridLayout.constraintCount = (deskData.boardSize.x >= deskData.boardSize.y) ? deskData.boardSize.x : deskData.boardSize.y;


        bool isWhite = !leftUpCellIsWhite;
        for (int y = 0; y < deskData.boardSize.y; y++)
        {
            if (deskData.boardSize.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < deskData.boardSize.x; x++)
            {
                GameObject cell = Instantiate(isWhite ? whiteCell : blackCell, boardGridLayout.transform);
                isWhite = !isWhite;

                if (!cell.TryGetComponent(out CellHandler cellHandler))
                    cellHandler = cell.AddComponent<CellHandler>();

                boardManager.cells[x, y] = cellHandler;
                cellHandler.Init(x, y);

                AddCallbackListener(cellHandler);
            }
        }
    }

    public void AddCallbackListener(CellHandler cellHandler)
    {
        cellHandler.OnPiecePlaced += CellCallback;
    }

    private void CellCallback(CellHandler cellHandler, PieceHandler pieceHandler)
    {
        pieceManager.MovePiece(pieceHandler, cellHandler);
    }
}
