using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PieceHandler : MonoBehaviour
{
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

        gameManager.PieceStartDrag(this);
    }
    public void OnDrag(PointerEventData eventData) => SetDraggingData(eventData);
    public void OnEndDrag(PointerEventData eventData, CellHandler startCell)
    {
        isDragging = false;

        MoveAttempt(eventData, startCell);
        gameManager.PieceEndDrag(this, parentCell);
    }

    private void Drag()
    {
        if (GetCellUnderPiece(lastDragEventData, out CellHandler cellHandler))
        {
            Vector3 position = Vector3.Lerp(rectTransform.position, draggedPosition, gameManager.PiecesSkinData.AnimationData.magnetToMouseLerpValue * Time.unscaledDeltaTime);
            gameManager.PieceDragging(this, position, cellHandler);
        }
    }

    private bool GetCellUnderPiece(PointerEventData eventData, out CellHandler cellHandler)
    {
        cellHandler = null;

        List<RaycastResult> results = new();
        graphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject != this && result.gameObject.TryGetComponent(out cellHandler))
                return true;
        }

        return false;
    }
    private void SetDraggingData(PointerEventData data)
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
                if (gameManager.IsMoveAllowed(this, startCell, cellHandler))
                {
                    parentCell = cellHandler.transform;

                    gameManager.MovePiece(this, startCell, cellHandler);
                }
            }
            else
            {
                //this.InactiveLog("Piece was moved to it's cell");
            }
        }
        else
        {
            //this.InactiveLog("Piece was moved not on a board");
        }

        transform.SetParent(parentCell, true);
    }
}
