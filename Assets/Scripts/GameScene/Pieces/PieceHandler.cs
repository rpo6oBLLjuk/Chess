using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PieceHandler : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameController gameController;

    public PieceData PieceData { get; private set; }

    private PieceAnimationData pieceAnimationData;

    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    private Transform parentCell;

    bool isDragging = false;


    public void Init(PieceAnimationData pieceAnimationData, PieceData pieceData, CellHandler cellHandler)
    {
        this.pieceAnimationData = pieceAnimationData;
        this.PieceData = pieceData;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentCell = transform.parent;

        transform.SetParent(canvas.transform);

        isDragging = true;
        transform.DOScale(Vector3.one * pieceAnimationData.scaleMultiplier, pieceAnimationData.scaleDuration);
    }

    public void OnDrag(PointerEventData eventData) => SetDraggedPosition(eventData);
    private void SetDraggedPosition(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out Vector3 globalMousePos))
            rectTransform.position = globalMousePos;
    }

    public void OnEndDrag(PointerEventData eventData, CellHandler startCell)
    {
        if (GetCell(eventData, out CellHandler cellHandler) && cellHandler != startCell)
        {
            if (gameController.CanBeMove(this, startCell, cellHandler))
            {
                gameController.MovePiece(this, startCell, cellHandler);

                parentCell = cellHandler.transform;
            }
        }

        transform.DOMove(parentCell.position, pieceAnimationData.magnetDuration)
            .OnComplete(() => transform.SetParent(parentCell));

        isDragging = false;
        transform.DOScale(Vector3.one, pieceAnimationData.scaleDuration);
    }

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

        notificationService.ShowPopup("Piece not on a board", "Piece Handler", PopupType.Warning);
        return false;
    }
}
