using CustomInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Panel : MonoBehaviour
{
    [Inject] protected PanelManager panelManager;

    public Button HideButton => hideButton;

    [Header("Panel settings")]
    [field: SerializeField] public bool Hideable = true; 
    [SerializeField, ShowIf(nameof(Hideable))] protected Button hideButton;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [SerializeField] protected bool autoHideOnAwake = true;


    protected virtual void Start()
    {
        if (autoHideOnAwake)
            ForceHide();
        hideButton?.onClick.AddListener(DisableCanvasGroup);
    }

    public virtual void ForceShow()
    {
        EnableCanvasGroup();
        CanvasGroup.alpha = 1.0f;
    }
    public virtual void ForceHide()
    {
        DisableCanvasGroup();
        CanvasGroup.alpha = 0.0f;
    }

    protected void EnableCanvasGroup() => SetCanvasGroupState(true);
    protected void DisableCanvasGroup() => SetCanvasGroupState(false);

    private void SetCanvasGroupState(bool value)
    {
        CanvasGroup.interactable = value;
        CanvasGroup.blocksRaycasts = value;
    }
}
