using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PieceEffectController : MonoBehaviour
{
    [SerializeField] private Image image;


    public void OnInitialized(float duration)
    {
        Sequence tween = DOTween.Sequence();

        tween.Append(image.DOFade(1, duration)
            .From(0));
        tween.Join(transform.DOScale(transform.localScale, duration)
            .From(Vector3.zero));

        tween.Play();
    }

    public void Destroy(float duration)
    {
        Sequence tween = DOTween.Sequence();

        tween.Append(image.DOFade(0, duration));
        tween.Join(transform.DOScale(Vector3.one * 1.25f, duration));
        tween.OnComplete(() => Destroy(gameObject));

        tween.Play();
    }

    private void Reset() => image = GetComponent<Image>();
}
