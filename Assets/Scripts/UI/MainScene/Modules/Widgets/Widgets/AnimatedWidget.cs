using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedWidget : Widget
{
    [SerializeField] private List<AnimatedWidgetElement> AnimatedWidgetElements;


    private void Awake()
    {
        AnimatedWidgetElements.ForEach(element => element.Initialize());
    }

    public override Tween ShowWidget(float showDuration)
    {
        AnimatedWidgetElements.ForEach(element => element.Show(showDuration));

        return base.ShowWidget(showDuration);
    }
    public override Tween HideWidget(float hideDuration)
    {
        AnimatedWidgetElements.ForEach(element => element.Hide(hideDuration));

        return base.HideWidget(hideDuration);
    }
}
