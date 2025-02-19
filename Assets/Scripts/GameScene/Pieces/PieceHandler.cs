using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PieceHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Inject] NotificationService notificationService;
    [Inject] PieceService pieceService;
    [Inject] GameController gameController;

    public PieceData PieceData { get; private set; }
    public CellHandler PreviousCell { get; private set; }
    public CellHandler CurrentCell { get; private set; }

    private PieceAnimationData pieceAnimationData;

    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    private Transform parentCell;


    public void Init(PieceAnimationData pieceAnimationData, PieceData pieceData, CellHandler cellHandler)
    {
        this.pieceAnimationData = pieceAnimationData;
        this.PieceData = pieceData;

        PreviousCell = CurrentCell = cellHandler;
    }

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
        parentCell = transform.parent;

        if (GetCell(eventData, out CellHandler cellHandler))
            CurrentCell = cellHandler;
        transform.SetParent(GetComponentInParent<Canvas>().transform);
    }

    public void OnDrag(PointerEventData eventData) => SetDraggedPosition(eventData);
    private void SetDraggedPosition(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out Vector3 globalMousePos))
            rectTransform.position = globalMousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetCell(eventData, out CellHandler cellHandler) && CurrentCell != cellHandler)
        {
            PreviousCell = CurrentCell;
            CurrentCell = cellHandler;

            if(pieceService.CanBeMove(this))
            {
                parentCell = cellHandler.transform;
                cellHandler.PiecePlaced(this);

                gameController.MovePiece(this);
            }
            else
            {
                CurrentCell = PreviousCell;
            }
        }

        transform.DOMove(parentCell.position, pieceAnimationData.magnetDuration)
            .OnComplete(() => transform.SetParent(parentCell));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * pieceAnimationData.scaleMultiplier, pieceAnimationData.scaleDuration);
        gameController.DownOnPiece(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, pieceAnimationData.scaleDuration);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        gameController.ClickOnPiece(this);
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
