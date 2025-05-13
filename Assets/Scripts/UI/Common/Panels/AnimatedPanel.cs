using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPanel : Panel
{
    [Header("Animated Panel settings")]
    [SerializeField] private AnimatedPanelData data;
    [SerializeField] private List<AnimatedElement> AnimatedWidgetElements;

    private bool initialized = false;


    public void Awake()
    {
        if (initialized)
            return;
        initialized = true;

        AnimatedWidgetElements.ForEach(element => element.Awake());
    }

    protected override void Start()
    {
        base.Start();
        hideButton?.onClick.AddListener(AnimHide);
    }

    public virtual void AnimShow()
    {
        panelManager?.PanelShowed(this);

        EnableCanvasGroup();

        CanvasGroup.DOFade(1, data.showDuration)
            .From(0);

        AnimatedWidgetElements.ForEach(element => element.Show(data.showDuration));
    }
    public virtual void AnimHide()
    {
        panelManager?.PanelHided(this);

        DisableCanvasGroup();

        CanvasGroup.DOFade(0, data.showDuration)
            .From(1);

        AnimatedWidgetElements.ForEach(element => element.Hide(data.showDuration));
    }

    public override void ForceShow()
    {
        base.ForceShow();

        panelManager?.PanelShowed(this);

        AnimatedWidgetElements.ForEach(element => element.Show(forceShow: true));
    }
    public override void ForceHide()
    {
        base.ForceHide();

        panelManager?.PanelHided(this);

        AnimatedWidgetElements.ForEach(element => element.Hide(forceHide: true));
    }

    private void OnDisable() => CanvasGroup.DOKill(this);
}
