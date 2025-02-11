using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public PieceAnimationData PieceAnimationData { get; set; }

    public CellHandler PreviousCell { get; private set; }

    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    private Transform previousParent;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();


        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
        {
            Debug.LogError("Canvas должен быть в режиме Screen Space - Camera!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        if(GetCell(eventData, out CellHandler cellHandler))
            PreviousCell = cellHandler;
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
        if (!GetCell(eventData, out CellHandler cellHandler))
            return;

        cellHandler.PiecePlaced(this);

        previousParent = cellHandler.transform;
        transform.SetParent(previousParent);
        transform.DOLocalMove(Vector3.zero, PieceAnimationData.magnetDuration);
    }

    public void OnPointerDown(PointerEventData eventData) => transform.DOScale(Vector3.one * PieceAnimationData.scaleMultiplier, PieceAnimationData.scaleDuration);
    public void OnPointerUp(PointerEventData eventData) => transform.DOScale(Vector3.one, PieceAnimationData.scaleDuration);

    private bool GetCell(PointerEventData eventData, out CellHandler cellHandler)
    {
        cellHandler = null;

        List<RaycastResult> results = new();
        graphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject != this && result.gameObject.TryGetComponent(out cellHandler))
            {
                return true;
            }
        }

        Debug.LogError("Piece not have cell");
        return false;
    }
}
