using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] public GraphicRaycaster graphicRaycaster { get; set; }

    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private float scaleDuration = 0.25f;

    [SerializeField] private float magnetDuration = 0.1f;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform prevousParent;


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
        prevousParent = transform.parent;
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
                prevousParent = result.gameObject.transform;
                cellHandler.PiecePlaced(this.gameObject);
            }
        }

        transform.SetParent(prevousParent);
        transform.localPosition = Vector3.zero; 


        ///////DO local position with magnetDuration
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * scaleMultiplier, scaleDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, scaleDuration);
    }
}
