using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    [field: SerializeField] public Button HideButton { get; private set; }


    public virtual void Initialize() { }

    public virtual void Show() => SetCanvasGroupState(true);
    public virtual void Hide() => SetCanvasGroupState(false);

    private void SetCanvasGroupState(bool value)
    {
        CanvasGroup.interactable = value;
        CanvasGroup.blocksRaycasts = value;
    }
}
