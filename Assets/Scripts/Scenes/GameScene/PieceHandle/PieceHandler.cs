using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PieceHandler : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameManager gameManager;

    [field: SerializeField]
    public PieceEffectHandler PieceEffectController { get; private set; }

    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    private Transform parentCell;

    bool isDragging = false;
    Vector3 draggedPosition;
    PointerEventData lastDragEventData;

    CellHandler previousCellUnderPiece;


    public void Init()
    {

    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    private void Update()
    {
        if (isDragging)
            Drag();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        parentCell = transform.parent;

        transform.SetParent(canvas.transform);

        isDragging = true;
        transform.DOScale(Vector3.one * gameManager.PiecesSkinData.AnimationData.scaleMultiplier, gameManager.PiecesSkinData.AnimationData.scaleDuration);
    }
    public void OnDrag(PointerEventData eventData) => SetDraggedData(eventData);
    public void OnEndDrag(PointerEventData eventData, CellHandler startCell)
    {
        MoveAttempt(eventData, startCell);

        transform.DOMove(parentCell.position, gameManager.PiecesSkinData.AnimationData.magnetToCellDuration)
            .OnComplete(() => transform.SetParent(parentCell));

        isDragging = false;
        transform.DOScale(Vector3.one, gameManager.PiecesSkinData.AnimationData.scaleDuration);
    }

    private void Drag()
    {
        transform.position = Vector3.Lerp(rectTransform.position, draggedPosition, gameManager.PiecesSkinData.AnimationData.magnetToMouseLerpValue * Time.unscaledDeltaTime);
        if (GetCellUnderPiece(lastDragEventData, out CellHandler cellHandler))
            gameManager.PieceDragging(this, cellHandler);
    }

    private bool GetCellUnderPiece(PointerEventData eventData, out CellHandler cellHandler)
    {
        cellHandler = null;

        List<RaycastResult> results = new();
        graphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject != this && result.gameObject.TryGetComponent(out cellHandler))
            {
                previousCellUnderPiece = cellHandler;
                return true;
            }
        }

        return false;
    }

    private void SetDraggedData(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out Vector3 globalMousePos))
            draggedPosition = globalMousePos;

        lastDragEventData = data;
    }
    private void MoveAttempt(PointerEventData eventData, CellHandler startCell)
    {
        if (GetCellUnderPiece(eventData, out CellHandler cellHandler))
        {
            if (cellHandler != startCell)
            {
                if (gameManager.CanBeMove(this, startCell, cellHandler))
                {
                    gameManager.MovePiece(this, startCell, cellHandler);

                    parentCell = cellHandler.transform;
                }
            }
            else
            {
                notificationService.ShowPopup("Piece was moved to it's cell", "Piece Handler", PopupType.Warning);
            }
        }
        else
        {
            notificationService.ShowPopup("Piece not on a board", "Piece Handler", PopupType.Warning);
        }

        gameManager.PressUpOnCell(previousCellUnderPiece);
    }
}
