using System;
using UnityEngine;

public class NotificationService : MonoService
{
    public Action<AnimatedPanel> DialogShowRequest;

    public PopupData popupData;
    public DialogData dialogData;

    public Transform popupParent;
    public Canvas dialogCanvas;

    private PopupMessageController popupMessageController;
    private DialogController dialogController;


    public override void OnInstantiated()
    {
        popupMessageController = new(popupData, popupParent);
        dialogController = container.Instantiate<DialogController>();
        dialogController.Init(dialogData, dialogCanvas.transform);
    }

    public void ShowPopup(string message, string sender = default, PopupType popupType = PopupType.None) => popupMessageController.Show(message, sender, popupType);
    public void ShowDialog(Action<bool> callback, string message, string sender = default, DialogType dialogType = DialogType.OkCancel)
    {
        AnimatedPanel dialogPanel = dialogController.ShowDialog(callback, message, sender, dialogType);
        DialogShowRequest?.Invoke(dialogPanel);
    }
}

public enum PopupType
{
    None,
    Info,
    Warning,
    Error
}

public enum DialogType
{
    OkCancel
}
