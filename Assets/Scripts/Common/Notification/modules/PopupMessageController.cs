using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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

    public void Show(string message, PopupType popupType = PopupType.None, Action showCallback = null, Action hideCallback = null)
    {
        GameObject popup = AddToList(data.popup);
        ConfiguratePopup(popup, message, popupType);
        AnimatePopup(popup, showCallback, hideCallback);
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
    private void AnimatePopup(GameObject popup, Action showCallback = null, Action hideCallback = null)
    {
        CanvasGroup canvasGroup = popup.GetComponentInChildren<CanvasGroup>();
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        Vector2 initialSize = rectTransform.sizeDelta;

        rectTransform.sizeDelta = Vector2.zero;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(canvasGroup.DOFade(1, data.showTime))
                .Join(DOTween.To(() => rectTransform.sizeDelta, x => rectTransform.sizeDelta = x, initialSize, data.showTime))
                .OnComplete(() => showCallback?.Invoke())
                .AppendInterval(data.duration)
                .Append(canvasGroup.DOFade(0, data.hideTime))
                .Join(DOTween.To(() => rectTransform.sizeDelta, x => rectTransform.sizeDelta = x, Vector2.zero, data.hideTime))
                .OnComplete(() => {
                    hideCallback?.Invoke();
                    DestroyPopup(popup);
                });

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
        popup.transform.DOKillAllTweens();
        popupList.Remove(popup);
        UnityEngine.Object.Destroy(popup);
    }
}
