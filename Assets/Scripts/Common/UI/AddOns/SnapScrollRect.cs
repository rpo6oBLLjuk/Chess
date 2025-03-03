using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(ScrollRect))]
public class SnapScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public event Action<Vector2Int> IndexChanged;

    [SerializeField] private ScrollRect scrollRect;

    public int CurrentHorizontalIndex
    {
        get => currentHorizontalIndex;
        set => currentHorizontalIndex = horizontalElementsCount > 0
                ? Mathf.Clamp(value, 0, horizontalElementsCount - 1)
                : 0;
    }
    [SerializeField] private int currentHorizontalIndex;

    public int CurrentVerticalIndex
    {
        get => currentVerticalIndex;
        set => currentVerticalIndex = verticalElementsCount > 0
                ? Mathf.Clamp(value, 0, verticalElementsCount - 1)
                : 0;
    }
    [SerializeField] private int currentVerticalIndex;

    public int HorizontalElementsCount => horizontalElementsCount;
    [SerializeField] private int horizontalElementsCount = 0;

    public int VerticalElementsCount => verticalElementsCount;
    [SerializeField] private int verticalElementsCount = 0;

    [Header("Values")]
    [SerializeField] private float smoothness = 10f;
    [SerializeField] private float scrollWeight = 0.01f;
    [SerializeField] private float endLerpValue = 0.000001f;

    [Header("Settings")]
    [SerializeField] private bool countingInactiveChildren = false;

    [SerializeField] private bool logging = false;

    private Vector2 targetPosition;
    private float hPerPage;
    private float vPerPage;
    private bool dragging;
    private bool forcePositionUpdate = false;

    private bool started = false;

    public bool Horizontal => scrollRect.horizontal;
    public bool Vertical => scrollRect.vertical;

    public void ScrollTo(int x, int y)
    {
        CurrentHorizontalIndex = x;
        CurrentVerticalIndex = y;
        forcePositionUpdate = true;
    }

    public void IncreasePosition(int x, int y)
    {
        CurrentHorizontalIndex += x;
        CurrentVerticalIndex += y;
        forcePositionUpdate = true;
    }
    public void DecreasePosition(int x, int y)
    {
        CurrentHorizontalIndex -= x;
        CurrentVerticalIndex -= y;
        forcePositionUpdate = true;
    }


    public void Start()
    {
        UpdateElementCount();

        targetPosition = GetSnapPosition();

        if (logging && !started)
            Debug.Log($"SnapScrolRect выполнил конфигурацию параметров", this);

        started = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UpdateIndex();
        targetPosition = GetSnapPosition();
        dragging = false;

        IndexChanged?.Invoke(new(CurrentHorizontalIndex, CurrentVerticalIndex));
    }

    [ExecuteAlways]
    public void LateUpdate()
    {
        UpdateElementCount();

        if (!dragging && scrollRect.normalizedPosition != targetPosition)
        {
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, targetPosition, smoothness * Time.unscaledDeltaTime);

            if (Vector2.Distance(scrollRect.normalizedPosition, targetPosition) < endLerpValue)
            {
                scrollRect.normalizedPosition = targetPosition;
            }
        }

        if (forcePositionUpdate)
        {
            forcePositionUpdate = false;
            targetPosition = GetSnapPosition();
        }
    }

    private void UpdateIndex()
    {
        float xPage, yPage = -1;

        if (Horizontal && horizontalElementsCount > 0)
        {
            xPage = (scrollRect.normalizedPosition.x / hPerPage);
            float diff = xPage - (1 * CurrentHorizontalIndex);

            if (diff >= scrollWeight)
            {
                CurrentHorizontalIndex++;
            }
            else if (diff <= -scrollWeight)
            {
                CurrentHorizontalIndex--;
            }
        }

        if (Vertical && verticalElementsCount > 0)
        {
            yPage = scrollRect.normalizedPosition.y / vPerPage;
            float diff = yPage - (1 * CurrentVerticalIndex);

            if (diff >= scrollWeight)
            {
                CurrentVerticalIndex++;
            }
            else if (diff <= -scrollWeight)
            {
                CurrentVerticalIndex--;
            }
        }
    }

    private Vector2 GetSnapPosition()
    {
        if (logging)
        {
            Debug.Log($"ElementsCount: {horizontalElementsCount}, HPerPage: {hPerPage}");
        }

        return new Vector2(Horizontal && horizontalElementsCount > 0 ? CurrentHorizontalIndex * hPerPage : scrollRect.normalizedPosition.x,
                           Vertical && verticalElementsCount > 0 ? CurrentVerticalIndex * vPerPage : scrollRect.normalizedPosition.y);
    }

    private void UpdateElementCount()
    {
        int contentCount = scrollRect.content.transform.GetChildCount(countingInactiveChildren);

        if (Horizontal && horizontalElementsCount != contentCount)
        {
            if (logging)
                Debug.Log($"{gameObject.name}: Horizontal Elements Count принудительно изменено с {horizontalElementsCount} на {contentCount}", this);

            horizontalElementsCount = contentCount;

            UpdateIndex();
        }
        else if (Vertical && verticalElementsCount != contentCount)
        {
            if (logging)
                Debug.Log($"Vertical Elements Count принудительно изменено с {verticalElementsCount} на {contentCount}", this);

            verticalElementsCount = contentCount;

            UpdateIndex();
        }

        hPerPage = 1f / (float)(horizontalElementsCount - 1);
        vPerPage = 1f / (float)(horizontalElementsCount - 1);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CurrentHorizontalIndex = currentHorizontalIndex;
        CurrentVerticalIndex = currentVerticalIndex;
        targetPosition = GetSnapPosition();
    }

    private void Reset()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
#endif
}