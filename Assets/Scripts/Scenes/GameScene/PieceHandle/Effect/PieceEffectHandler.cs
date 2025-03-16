using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PieceEffectHandler : MonoBehaviour
{
    [SerializeField] private Image image;


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

    public void Destroy(float duration)
    {
        gameObject.transform.SetParent(transform.root);
        image.raycastTarget = false;

        Sequence tween = DOTween.Sequence(transform);

        tween.Append(image.DOFade(0, duration));
        tween.Join(transform.DOScale(Vector3.one * 1.25f, duration));
        tween.OnComplete(() => Destroy(gameObject));

        tween.Play();
    }

    private void Reset() => image = GetComponent<Image>();
}
