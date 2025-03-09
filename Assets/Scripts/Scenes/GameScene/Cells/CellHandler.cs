using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CellHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    public byte Index { get; private set; }
    [field: SerializeField] public CellEffectController CellEffectController { get; private set; }

    [Inject] GameController gameController;

    private PieceHandler CurrentPieceHandler;


    public void Init(byte index) => this.Index = index;

    public void PiecePlaced(PieceHandler pieceHandler) => CurrentPieceHandler = pieceHandler;
    public void PieceRemoved() => CurrentPieceHandler = null;

    public void OnPointerClick(PointerEventData eventData) => gameController.ClickOnCell(this);
    public void OnPointerDown(PointerEventData eventData) => gameController.PressDownOnCell(this);

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentPieceHandler?.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        CurrentPieceHandler?.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentPieceHandler?.OnEndDrag(eventData, this);
    }
}
