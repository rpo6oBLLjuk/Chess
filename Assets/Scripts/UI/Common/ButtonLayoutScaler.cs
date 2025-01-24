using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ButtonLayoutScaler : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Vector2 newFlexibleSize = Vector2.one;
    [SerializeField] private float duration = 0.1f;

    private Vector2 defaultFlexibleSize;

    private void Awake()
    {
        defaultFlexibleSize = new Vector2(layoutElement.flexibleWidth, layoutElement.flexibleHeight);
    }

    public void OnPointerDown(PointerEventData eventData) => layoutElement.DOFlexibleSize(newFlexibleSize, duration);
    public void OnPointerExit(PointerEventData eventData) => layoutElement.DOFlexibleSize(defaultFlexibleSize, duration);

    private void Reset() => layoutElement = GetComponent<LayoutElement>();
}
