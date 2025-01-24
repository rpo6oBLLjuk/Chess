using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonSimpleScaler : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 newLocalScale = Vector3.one;
    [SerializeField] private float duration = 0.1f;

    private Vector3 defaultScale;

    private void Awake() => defaultScale = transform.localScale;

    public void OnPointerDown(PointerEventData eventData) => transform.DOScale(newLocalScale, duration);
    public void OnPointerExit(PointerEventData eventData) => transform.DOScale(defaultScale, duration);
}
