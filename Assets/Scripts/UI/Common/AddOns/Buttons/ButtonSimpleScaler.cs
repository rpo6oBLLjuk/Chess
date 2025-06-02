using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ButtonSimpleScaler : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] protected Button button;

    [SerializeField] protected Vector3 newLocalScale = Vector3.one;
    [SerializeField] protected float duration = 0.1f;

    protected Vector3 defaultScale;


    protected virtual void Awake() => defaultScale = transform.localScale;

    public void OnPointerDown(PointerEventData eventData) => transform.DOScale(newLocalScale, duration);
    public void OnPointerExit(PointerEventData eventData) => transform.DOScale(defaultScale, duration);

    protected virtual void OnDisable() => transform.DOKill();

    private void Reset() => button = GetComponent<Button>();
}
