using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Button HideButton => hideButton;

    [Header("Panel settings")]
    [SerializeField] protected Button hideButton;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }


    protected virtual void Start() => hideButton?.onClick.AddListener(Hide);

    public virtual void Initialize() { }

    public virtual void Show() => SetCanvasGroupState(true);
    public virtual void Hide() => SetCanvasGroupState(false);

    private void SetCanvasGroupState(bool value)
    {
        CanvasGroup.interactable = value;
        CanvasGroup.blocksRaycasts = value;
    }
}
