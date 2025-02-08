using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutGroupAutosize : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private RectTransform rectTransform;

    [Header("Settings")]
    [SerializeField] private bool isSquare = true;
    [SerializeField] private bool countingInactiveChildren = false;

    [Header("Debug")]
    [SerializeField] private bool logging = false;

    private struct GridLayoutGroupState
    {
        public GridLayoutGroup.Constraint constraint;
        public int constraintCount;
        public RectOffset padding;
        public Vector2 cellSize;
        public Vector2 spacing;
        public TextAnchor childAlignment;
        public bool startAxisVertical;
        public int childCount;
        public bool isSquare;
        public bool countingInactiveChildren;
    }
    private struct RectTransformState
    {
        public Vector2 anchoredPosition;
        public Vector2 size;
        public Vector2 sizeDelta;
        public Vector3 localScale;
        public Quaternion localRotation;
    }

    private GridLayoutGroupState previousGridState;
    private RectTransformState previousRectState;

    private void Start()
    {
        if (!TryGetComponent(out rectTransform))
            Debug.LogError("RectTransform not found", this);

        if (!TryGetComponent(out gridLayoutGroup))
            Debug.LogError("GridLayoutGroup not found", this);
    }

    [ExecuteAlways]
    private void Update() => Refresh();

    private void Refresh()
    {
        if (HasStateChanged())
        {
            StoreCurrentState();

            SetSize();
        }
    }

    private void StoreCurrentState()
    {
        previousGridState = new GridLayoutGroupState
        {
            constraint = gridLayoutGroup.constraint,
            constraintCount = gridLayoutGroup.constraintCount,
            padding = new RectOffset(
                gridLayoutGroup.padding.left,
                gridLayoutGroup.padding.right,
                gridLayoutGroup.padding.top,
                gridLayoutGroup.padding.bottom),
            cellSize = gridLayoutGroup.cellSize,
            spacing = gridLayoutGroup.spacing,
            childAlignment = gridLayoutGroup.childAlignment,
            startAxisVertical = (gridLayoutGroup.startAxis == GridLayoutGroup.Axis.Vertical),
            childCount = GetChildren(),
            isSquare = this.isSquare,
            countingInactiveChildren = this.countingInactiveChildren,
        };

        previousRectState = new RectTransformState
        {
            anchoredPosition = rectTransform.anchoredPosition,
            size = new Vector2(rectTransform.rect.width, rectTransform.rect.height),
            sizeDelta = rectTransform.sizeDelta,
            localScale = rectTransform.localScale,
            localRotation = rectTransform.localRotation
        };
    }
    private bool HasStateChanged()
    {
        if (previousGridState.constraint != gridLayoutGroup.constraint ||
            previousGridState.constraintCount != gridLayoutGroup.constraintCount ||
            !AreRectOffsetsEqual(previousGridState.padding, gridLayoutGroup.padding) ||
            previousGridState.cellSize != gridLayoutGroup.cellSize ||
            previousGridState.spacing != gridLayoutGroup.spacing ||
            previousGridState.childAlignment != gridLayoutGroup.childAlignment ||
            previousGridState.startAxisVertical != (gridLayoutGroup.startAxis == GridLayoutGroup.Axis.Vertical ||
            previousGridState.childCount != GetChildren() ||
            previousGridState.isSquare != isSquare ||
            previousGridState.countingInactiveChildren != countingInactiveChildren))
        {
            return true;
        }

        if (previousRectState.anchoredPosition != rectTransform.anchoredPosition ||
            previousRectState.size != new Vector2(rectTransform.rect.width, rectTransform.rect.height) ||
            previousRectState.sizeDelta != rectTransform.sizeDelta ||
            previousRectState.localScale != rectTransform.localScale ||
            previousRectState.localRotation != rectTransform.localRotation)
        {
            return true;
        }

        return false;
    }

    private bool AreRectOffsetsEqual(RectOffset a, RectOffset b)
    {
        return a.left == b.left &&
               a.right == b.right &&
               a.top == b.top &&
               a.bottom == b.bottom;
    }

    private void SetSize()
    {
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.Flexible)
        {
            Debug.LogError("GridLayoutGroup constraint type is not fixed");
            return;
        }

        CalculateCellSize();

        if (logging)
            Debug.Log("Resized", this);
    }
    private void CalculateCellSize()
    {
        float verticalPadding = gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top;
        float horizontalPadding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float verticalSpacing = gridLayoutGroup.spacing.y;
        float horizontalSpacing = gridLayoutGroup.spacing.x;

        float childCount = GetChildren();
        float countOnOtherAxis = Mathf.Ceil(childCount / (float)gridLayoutGroup.constraintCount);

        countOnOtherAxis = countOnOtherAxis > 0 ? countOnOtherAxis : 1;

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

        if (isSquare)
        {
            float cellSize = Mathf.Min(minWidth, minHeight);
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        }
        else
        {
            gridLayoutGroup.cellSize = new Vector2(minWidth, minHeight);
        }

    }

    private int GetChildren()
    {
        if (countingInactiveChildren)
        {
            return gridLayoutGroup.transform.childCount;
        }
        else
        {
            int activeChildCount = 0;

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    activeChildCount++;
                }
            }

            return activeChildCount;
        }
    }

    private void Reset()
    {
        if (!TryGetComponent(out rectTransform))
            Debug.LogError("RectTransform not found", this);

        if (!TryGetComponent(out gridLayoutGroup))
            Debug.LogError("GridLayoutGroup not found", this);
    }
}
