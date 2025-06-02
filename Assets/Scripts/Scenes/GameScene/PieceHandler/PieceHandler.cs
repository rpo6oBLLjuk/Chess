using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PieceHandler : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] SkinData skinData;

    [field: SerializeField]
    public PieceEffectHandler PieceEffectController { get; private set; }

    private RectTransform rectTransform;
    private GraphicRaycaster graphicRaycaster;

    bool isDragging = false;
    Vector3 draggedPosition;
    PointerEventData lastDragEventData;


    public void Init()
    {

    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        gameManager.PieceStartDrag(this);
    }
    public void OnDrag(PointerEventData eventData) => SetDraggingData(eventData);
    public void OnEndDrag(PointerEventData eventData, CellHandler startCell)
    {
        isDragging = false;

        if (GetCellUnderPiece(eventData, out CellHandler endCell))
        {
            gameManager.PieceEndDrag(this, endCell);
        }
    }

    private void Drag()
    {
        GetCellUnderPiece(lastDragEventData, out CellHandler cellHandler);
        Vector3 position = Vector3.Lerp(rectTransform.position, draggedPosition, skinData.pieceAnimationData.magnetToMouseLerpValue * Time.unscaledDeltaTime);

        gameManager.PieceDragging(this, position, cellHandler);
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
}
