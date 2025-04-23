using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Button HideButton => hideButton;

    [Header("Panel settings")]
    [SerializeField] protected Button hideButton;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [SerializeField] protected bool autoHideOnAwake = true;

    protected bool initialized = false;


    protected virtual void Start()
    {
        Initialize();

        if (autoHideOnAwake)
            ForceHide();
        hideButton?.onClick.AddListener(DisableCanvasGroup);
    }

    public virtual void Initialize()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;
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
