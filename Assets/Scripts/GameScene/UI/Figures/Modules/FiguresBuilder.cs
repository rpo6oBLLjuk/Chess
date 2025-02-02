using UnityEngine;
using UnityEngine.UI;

public class FiguresBuilder : MonoBehaviour
{
    [SerializeField] private BoardCellBuilder boardCellBuilder;
    [SerializeField] private FigureObjectsData figureObjectsData;

    private Transform[,] figuresInstance;


    public void SetupFigures(DeskData data)
    {
        figuresInstance = new Transform[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];
        boardCellBuilder.BoardGridLayout.constraint = (boardCellBuilder.BoardSize.x > boardCellBuilder.BoardSize.y) ?
            GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;

        boardCellBuilder.BoardGridLayout.constraintCount = (boardCellBuilder.BoardSize.x >= boardCellBuilder.BoardSize.y) ? boardCellBuilder.BoardSize.x : boardCellBuilder.BoardSize.y;

        for (int y = 0; y < boardCellBuilder.BoardSize.y; y++)
        {
            for (int x = 0; x < boardCellBuilder.BoardSize.x; x++)
            {
                if (data.figureData[x, y] != null)
                {
                    GameObject figure = Instantiate(figureObjectsData.GetFigureByType(data.figureData[x, y]), boardCellBuilder.BoardCells[x, y]);
                    //figure.GetComponentInChildren<SpriteRenderer>.sprite = 
                    figuresInstance[x, y] = figure.transform;
                }
            }
        }
    }
}
