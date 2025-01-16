using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWidget : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button HideButton;


    public void InitWidget(float Duration) => HideButton.onClick.AddListener(() => HideWidget(Duration));

    public void ShowWidget(float duration)
    {
        canvasGroup.DOFade(1, duration)
            .From(0);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideWidget(float duration)
    {
        canvasGroup.DOFade(0, duration)
            .From(1);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
