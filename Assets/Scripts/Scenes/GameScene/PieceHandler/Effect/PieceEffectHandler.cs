using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PieceEffectHandler : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private UIEffect uiEffect;
    [SerializeField] private UIEffectTweener effectTweener;


    public void OnInitialized(float duration)
    {
        image ??= GetComponent<Image>();

        Sequence tween = DOTween.Sequence(transform);

        tween.Append(image.DOFade(1, duration)
            .From(0));
        tween.Join(transform.DOScale(transform.localScale, duration)
            .From(Vector3.zero));
        tween.Play();
    }

    public void Check(UIEffectPreset checkPreset)
    {
        uiEffect.LoadPreset(checkPreset);
        effectTweener.enabled = true;
    }
    public void ResolveCheck(UIEffectPreset defaultPreset)
    {
        uiEffect.LoadPreset(defaultPreset);
        effectTweener.enabled = false;
    }

    public void Destroy(float duration, AnimationCurve positionCurve)
    {
        gameObject.transform.SetParent(transform.root);
        RectTransform rectTransform = transform.GetComponentInChildren<RectTransform>();
        image.raycastTarget = false;

        Sequence tween = DOTween.Sequence(transform);

        tween.Join(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - 100, duration)
            .SetEase(positionCurve));

        tween.Join(image.DOFade(0, duration / 2)
            .SetDelay(duration / 2));
        tween.Join(transform.DOScale(Vector3.one * 0.5f, duration / 2));

        tween.OnComplete(() => Destroy(gameObject));
        tween.Play();
    }

    private void Reset() => image = GetComponent<Image>();
}
