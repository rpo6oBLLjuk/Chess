using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPanel : Panel
{
    [SerializeField] private List<AnimatedElement> AnimatedWidgetElements;


    public override void Initialize() => AnimatedWidgetElements.ForEach(element => element.Initialize());

    public void Show(float showDuration, float delay = 0)
    {
        base.Show();

        CanvasGroup.DOFade(1, showDuration)
            .SetDelay(delay);

        AnimatedWidgetElements.ForEach(element => element.Show(showDuration, delay));
    }
    public void Hide(float hideDuration, float delay = 0)
    {
        base.Hide();

        CanvasGroup.DOFade(0, hideDuration)
            .SetDelay(delay);

        AnimatedWidgetElements.ForEach(element => element.Hide(hideDuration, delay));
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
