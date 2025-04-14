using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Button HideButton => hideButton;

    [Header("Panel settings")]
    [SerializeField] protected Button hideButton;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [SerializeField] protected bool autoHideOnAwake = true;


    protected virtual void Start()
    {
        Initialize();

        if (autoHideOnAwake)
            ForceHide();
        hideButton?.onClick.AddListener(HideCanvasGroup);
    }

    public virtual void Initialize() { }

    public virtual void ForceShow()
    {
        ShowCanvasGroup();
        CanvasGroup.alpha = 1.0f;
    }
    public virtual void ForceHide()
    {
        HideCanvasGroup();
        CanvasGroup.alpha = 0.0f;
    }

    protected void ShowCanvasGroup() => SetCanvasGroupState(true);
    protected void HideCanvasGroup() => SetCanvasGroupState(false);

    private void SetCanvasGroupState(bool value)
    {
        CanvasGroup.interactable = value;
        CanvasGroup.blocksRaycasts = value;
    }
}
