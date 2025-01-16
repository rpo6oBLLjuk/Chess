using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutGroupAutosize : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    private RectTransform rectTransform;
    private Vector2 LastRectSize;


    private void Start()
    {
        if (!TryGetComponent(out rectTransform))
            Debug.LogError("RectTransform not found", this);

        if (!TryGetComponent(out gridLayoutGroup))
            Debug.LogError("GridLayoutGroup not found", this);
    }

    private void Update() => Refresh();

    private void Refresh()
    {
        if (rectTransform.rect.width != LastRectSize.x || rectTransform.rect.height != LastRectSize.y)
        {
            LastRectSize.x = rectTransform.rect.width;
            LastRectSize.y = rectTransform.rect.height;

            SetSize();
        }
    }

    private void SetSize()
    {
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.Flexible)
        {
            Debug.LogError("GridLayoutGroup constraint type is not fixed");
            return;
        }

        float cellSize = CalculateCellSize();
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        Debug.Log("Resized", this);
    }

    private float CalculateCellSize()
    {
        float verticalPadding = gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top;
        float horizontalPadding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float verticalSpacing = gridLayoutGroup.spacing.y;
        float horizontalSpacing = gridLayoutGroup.spacing.x;

        float childCount = gridLayoutGroup.transform.childCount;
        float countOnOtherAxis = Mathf.FloorToInt(childCount / gridLayoutGroup.constraintCount);

        float minWidth = 0;
        float minHeight = 0;

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            minWidth = (rectTransform.rect.width - (horizontalPadding + horizontalSpacing * (countOnOtherAxis - 1))) / countOnOtherAxis;
            minHeight = (rectTransform.rect.height - (verticalPadding + verticalSpacing * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
        }
        else if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            minWidth = (rectTransform.rect.width - (horizontalPadding + horizontalSpacing * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
            minHeight = (rectTransform.rect.height - (verticalPadding + verticalSpacing * (countOnOtherAxis - 1))) / countOnOtherAxis;
        }

        return Mathf.Min(minWidth, minHeight);
    }

    private void Reset()
    {
        if (!TryGetComponent(out rectTransform))
            Debug.LogError("RectTransform not found", this);

        if (!TryGetComponent(out gridLayoutGroup))
            Debug.LogError("GridLayoutGroup not found", this);
    }
}
