using UnityEngine;
using UnityEngine.UI;

public class BoardCellBuilder : MonoBehaviour
{
    public Vector2Int BoardSize { get; private set; }
    public Transform[,] BoardCells { get; private set; }
    public GridLayoutGroup BoardGridLayout => boardGridLayout;

    [Header("References")]
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private GameObject blackCell;
    [SerializeField] private GameObject whiteCell;

    [Header("Board data")]
    [SerializeField] private bool leftUpCellIsWhite = true;


    public void SetupBoard(DeskData deskData)
    {
        BoardSize = deskData.boardSize;

        BoardCells = new Transform[deskData.boardSize.x, deskData.boardSize.y]; //board Init

        boardGridLayout.constraint = (deskData.boardSize.x > deskData.boardSize.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;

        boardGridLayout.constraintCount = (deskData.boardSize.x >= deskData.boardSize.y) ? deskData.boardSize.x : deskData.boardSize.y;


        bool isWhite = leftUpCellIsWhite;
        for (int y = 0; y < deskData.boardSize.y; y++)
        {
            if (deskData.boardSize.x % 2 == 0)
                isWhite = !isWhite;

            for (int x = 0; x < deskData.boardSize.x; x++)
            {
                GameObject cell = Instantiate(isWhite ? whiteCell : blackCell, boardGridLayout.transform);
                BoardCells[x, y] = cell.transform;
                isWhite = !isWhite;

                if(!cell.TryGetComponent(out CellHandler cellHandler))
                    cellHandler = cell.AddComponent<CellHandler>();

                cellHandler.Init(x, y);
                AddCallbackListener(cellHandler);
            }
        }
    }

    public void AddCallbackListener(CellHandler cellHandler)
    {
        cellHandler.OnFigurePlaced += CellCallback;
    }

    private void CellCallback(Vector2Int index, GameObject figure)
    {
        Debug.Log($"Figure {figure.name} at cell {index} placed", figure);
    }
}
