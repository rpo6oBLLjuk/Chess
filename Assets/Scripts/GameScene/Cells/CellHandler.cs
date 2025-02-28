using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CellHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    [Inject] GameController gameController;

    [field: SerializeField] public CellEffectController CellEffectController { get; private set; }

    public Vector2Int CellIndex { get; private set; }
    public PieceHandler CurrentPieceHandler { get; private set; }

    public bool IsDragging => isDragging;
    private bool isDragging;


    public void Init(int x, int y)
    {
        CellIndex = new Vector2Int(x, y);
    }

    public void PiecePlaced(PieceHandler pieceHandler) => CurrentPieceHandler = pieceHandler;
    public void PieceRemoved() => CurrentPieceHandler = null;

    public void OnPointerClick(PointerEventData eventData) => gameController.ClickOnCell(this);
    public void OnPointerDown(PointerEventData eventData) => gameController.PointerDownOnCell(this);

    public void OnBeginDrag(PointerEventData eventData) => CurrentPieceHandler.OnBeginDrag(eventData);
    public void OnDrag(PointerEventData eventData) => CurrentPieceHandler.OnDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => CurrentPieceHandler.OnEndDrag(eventData, this);
}
