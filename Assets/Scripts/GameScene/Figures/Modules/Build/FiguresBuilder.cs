using UnityEngine;
using UnityEngine.UI;

public class FiguresBuilder : MonoBehaviour
{
    [SerializeField] private BoardCellBuilder boardCellBuilder;
    [SerializeField] private FiguresPooler pooler = new();
    [SerializeField] private FiguresSkinData figuresSkinData;

    private Transform[,] figuresInstance;


    public void SetupFigures(DeskData data)
    {
        data.figureData = new FigureData[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];

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
                    GameObject instance = Instantiate(data.figureData[x, y], boardCellBuilder.BoardCells[x, y]);
                    figuresInstance[x, y] = instance.transform;
                }
            }
        }
    }

    private GameObject Instantiate(FigureData figureData, Transform boardCell)
    {
        GameObject figure = Instantiate(pooler.Get(figureData.Type), boardCell);
        figure.GetComponentInChildren<SpriteRenderer>().sprite = figuresSkinData.Get(figureData.Type, figureData.Color);
        return figure;
    }
}
