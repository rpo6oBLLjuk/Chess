using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CellTargetController : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    private Sequence colorSequence;

    public void SetColor(Color color)
    {
        if (targetImage != null)
        {
            targetImage.color = color;
        }
    }

    public void SetAnimColor(Color color, float duration)
    {
        SetColor(color);

        colorSequence?.Kill();

        colorSequence = DOTween.Sequence();
        colorSequence.Append(targetImage.DOColor(new Color(color.r, color.g, color.b, 0), duration / 2))
            .Append(targetImage.DOColor(color, duration / 2))
            .SetLoops(-1)
            .Play();
    }
}
