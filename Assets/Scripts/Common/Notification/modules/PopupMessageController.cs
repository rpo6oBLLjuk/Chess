using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupMessageController
{
    private PopupData data;

    private List<GameObject> popupList = new();
    private Transform parent;


    public PopupMessageController(PopupData data, Transform parent)
    {
        this.data = data;
        this.parent = parent;
    }

    public void Show(string message, PopupType popupType = PopupType.None)
    {
        GameObject popup = AddToList(data.popup);
        ConfiguratePopup(popup, message, popupType);
        AnimatePopup(popup);
    }

    private void ConfiguratePopup(GameObject popup, string message, PopupType popupType = PopupType.None)
    {
        popup.transform.Find("PopupText").GetComponent<TextMeshProUGUI>().text = message;
        Image popupImg = popup.transform.Find("PopupIcon").GetComponent<Image>();

        popupImg.sprite = popupType switch
        {
            PopupType.None => null,
            PopupType.Info => data.infoSprite,
            PopupType.Warning => data.warningSprite,
            PopupType.Error => data.errorSprite,
            _ => null
        };

        popupImg.color = popupType switch
        {
            PopupType.None => new Color(0, 0, 0, 0),
            PopupType.Info => data.infoColor,
            PopupType.Warning => data.warningColor,
            PopupType.Error => data.errorColor,
            _ => new Color(0, 0, 0, 0)
        };

        if (popupType == PopupType.None)
        {
            popupImg.color = new Color(0, 0, 0, 0);
        }
    }
    private void AnimatePopup(GameObject popup)
    {
        CanvasGroup canvasGroup = popup.GetComponentInChildren<CanvasGroup>();
        RectTransform rectTransform = popup.GetComponent<RectTransform>();

        VerticalLayoutGroup verticalLayoutGroup = popup.GetComponentInParent<VerticalLayoutGroup>();
        RectTransform parentRectTransform = verticalLayoutGroup.GetComponent<RectTransform>();

        Vector3 initialScale = rectTransform.localScale;
        rectTransform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1, data.showTime))
                .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform))
                .Join(rectTransform.DOScale(initialScale, data.showTime)
                    .From(Vector3.zero))
                .AppendInterval(data.duration)
                .OnComplete(() => DestroyPopup(popup));
        sequence.Play();
    }

    private GameObject AddToList(GameObject popup)
    {
        if (popupList.FirstOrDefault() != null)
        {
            if (!data.useList || popupList.Count >= data.listSize)
            {
                DestroyPopup(popupList.First());
            }
        }
        GameObject instance = UnityEngine.Object.Instantiate(popup, parent);
        popupList.Add(instance);
        return instance;
    }
    private void DestroyPopup(GameObject popup)
    {
        popupList.Remove(popup);

        CanvasGroup canvasGroup = popup.GetComponentInChildren<CanvasGroup>();
        RectTransform rectTransform = popup.GetComponent<RectTransform>();

        VerticalLayoutGroup verticalLayoutGroup = popup.GetComponentInParent<VerticalLayoutGroup>();
        RectTransform parentRectTransform = verticalLayoutGroup.GetComponent<RectTransform>();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, data.hideTime))
                .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform))
                .Join(rectTransform.DOScaleY(0, data.hideTime))
                .OnComplete(() =>
                {
                    popup.transform.DOKillAllTweens();
                    UnityEngine.Object.Destroy(popup);
                });
        sequence.Play();
    }
}
