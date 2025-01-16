using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSwitcher : MonoBehaviour
{
    public List<WidgetContainer> widgets;

    [SerializeField] private float duration;


    private void OnEnable()
    {
        foreach (WidgetContainer widgetContainer in widgets)
        {
            widgetContainer.widget.InitWidget(duration);
            widgetContainer.showButton.onClick.AddListener(() => widgetContainer.widget.ShowWidget(duration));
        }
    }
}

[Serializable]
public class WidgetContainer
{
    public UIWidget widget;
    public Button showButton;
}
