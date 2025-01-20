using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Widget : MonoBehaviour
{
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    [field: SerializeField] public Button HideButton { get; private set; }


    public virtual Tween ShowWidget(float showDuration)
    {
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;

        return CanvasGroup.DOFade(1, showDuration);
    }

    public virtual Tween HideWidget(float hideDuration)
    {
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;

        return CanvasGroup.DOFade(0, hideDuration);

    }
}
