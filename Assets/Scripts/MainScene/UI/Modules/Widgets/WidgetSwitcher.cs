using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSwitcher : MonoBehaviour
{
    public CanvasGroup mainPanel;

    [Serializable]
    private class WidgetContainer
    {
        public UIWidget widget;
        public Button showButton;
    }
    [SerializeField] private List<WidgetContainer> widgets;

    [SerializeField] private float mainPanelShowDuration = 0.1f;
    [SerializeField] private float mainPanelHideDuration = 0.1f;

    [SerializeField] private float showDuration = 0.2f;
    [SerializeField] private float hideDuration = 0.2f;


    private void OnEnable()
    {
        foreach (WidgetContainer widgetContainer in widgets)
        {
            if (widgetContainer.widget == null)
                continue;

            widgetContainer.showButton.onClick.AddListener(() =>
            {
                mainPanel.DOFade(0, mainPanelHideDuration)
                          .OnComplete(() => widgetContainer.widget.ShowWidget(showDuration));
            });
            widgetContainer.widget.HideButton.onClick.AddListener(() =>
            {
                widgetContainer.widget.HideWidget(hideDuration)
                                        .OnComplete(() => mainPanel.DOFade(1, mainPanelShowDuration));
            });
            widgetContainer.widget.HideWidget(0);
        }
    }
}


