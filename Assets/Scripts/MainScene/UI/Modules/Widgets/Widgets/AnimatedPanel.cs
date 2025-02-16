using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPanel : Panel
{
    [Header("Animated Panel settings")]
    [SerializeField] private AnimatedPanelData data;
    [SerializeField] private List<AnimatedElement> AnimatedWidgetElements;


    public override void Initialize() => AnimatedWidgetElements.ForEach(element => element.Initialize());

    public override void Show()
    {
        base.Show();

        CanvasGroup.DOFade(1, data.showDuration)
            .From(0);

        AnimatedWidgetElements.ForEach(element => element.Show(data.showDuration));
    }
    public override void Hide()
    {
        base.Hide();

        CanvasGroup.DOFade(0, data.showDuration)
            .From(1);

        AnimatedWidgetElements.ForEach(element => element.Hide(data.showDuration));
    }

    public void ForceShow()
    {
        base.Show();

        CanvasGroup.alpha = 1;

        AnimatedWidgetElements.ForEach(element => element.Show(forceShow: true));
    }
    public void ForceHide()
    {
        base.Hide();

        CanvasGroup.alpha = 0;

        AnimatedWidgetElements.ForEach(element => element.Hide(forceHide: true));
    }
}
