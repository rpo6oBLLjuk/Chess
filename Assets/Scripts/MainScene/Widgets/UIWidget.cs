using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWidget : MonoBehaviour
{
    public float Duration;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button ShowButton;
    [SerializeField] private Button HideButton;


    private void OnEnable()
    {
        ShowButton.onClick.AddListener(() => ShowWidget(Duration));
        HideButton.onClick.AddListener(() => HideWidget(Duration));
    }

    private void Awake()
    {
        HideWidget(0);
    }

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
