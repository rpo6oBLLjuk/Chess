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

        Sequence animationSequence;

        canvasGroup.DOFade(1, data.showTime)
            .From(0)
            .OnComplete(() =>
            {
                showCallback?.Invoke();
                canvasGroup.DOFade(0, data.hideTime)
                    .From(1)
                    .SetDelay(data.duration)
                    .OnComplete(() =>
                    {
                        hideCallback?.Invoke();
                        DestroyPopup(popup);
                    });
            });
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
