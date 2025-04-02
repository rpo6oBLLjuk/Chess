using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CellHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    public byte Index { get; set; }
    [field: SerializeField] public CellEffectHandler CellEffectController { get; private set; }

    [Inject] GameManager gameManager;

    private PieceHandler CurrentPieceHandler;


    public void Init(byte index) => this.Index = index;

    public void PiecePlaced(PieceHandler pieceHandler) => CurrentPieceHandler = pieceHandler;
    public void PieceRemoved() => CurrentPieceHandler = null;

    public void OnPointerClick(PointerEventData eventData) => gameManager.ClickOnCell(this);
    public void OnPointerDown(PointerEventData eventData) => gameManager.PressDownOnCell(this);

    public void OnBeginDrag(PointerEventData eventData) => CurrentPieceHandler?.OnBeginDrag(eventData);
    public void OnDrag(PointerEventData eventData) => CurrentPieceHandler?.OnDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => CurrentPieceHandler?.OnEndDrag(eventData, this);
}
