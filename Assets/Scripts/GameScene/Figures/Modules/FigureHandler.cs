using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FigureHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] public GraphicRaycaster graphicRaycaster { get; set; }

    private RectTransform rectTransform;
    private Canvas canvas;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
        {
            Debug.LogError("Canvas должен быть в режиме Screen Space - Camera!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(GetComponentInParent<Canvas>().transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out Vector3 globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        graphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject != this && result.gameObject.TryGetComponent(out CellHandler cellHandler))
            {
                transform.SetParent(result.gameObject.transform);
                cellHandler.FigurePlaced(this.gameObject);
            }
        }

        transform.localPosition = Vector3.zero;
    }
}
