using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPanel : Panel
{
    [Header("Animated Panel settings")]
    [SerializeField] private AnimatedPanelData data;
    [SerializeField] private List<AnimatedElement> AnimatedWidgetElements;


    public override void Initialize() => AnimatedWidgetElements.ForEach(element => element.Initialize());

    protected override void Start()
    {
        base.Start();
        hideButton?.onClick.AddListener(AnimHide);
    }

    public virtual void AnimShow()
    {
        ShowCanvasGroup();

        CanvasGroup.DOFade(1, data.showDuration)
            .From(0);

        AnimatedWidgetElements.ForEach(element => element.Show(data.showDuration));
    }
    public virtual void AnimHide()
    {
        HideCanvasGroup();

        CanvasGroup.DOFade(0, data.showDuration)
            .From(1);

        AnimatedWidgetElements.ForEach(element => element.Hide(data.showDuration));
    }

    public override void ForceShow()
    {
       base .ForceShow();

        AnimatedWidgetElements.ForEach(element => element.Show(forceShow: true));
    }
    public override void ForceHide()
    {
       base.ForceHide();

        AnimatedWidgetElements.ForEach(element => element.Hide(forceHide: true));
    }

    private void OnDestroy() => CanvasGroup.DOKill(this);
}
