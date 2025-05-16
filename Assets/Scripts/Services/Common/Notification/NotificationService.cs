using System;
using UnityEngine;


public class NotificationService : MonoService
{
    public PopupData popupData;
    public DebugMessageControllerData debugMessageControllerData;
    public DialogData dialogData;

    public Transform popupParent;
    public Canvas dialogCanvas;

    private PopupMessageController popupMessageController;
    private DialogController dialogController;

    private DebugMessageController debugMessageController;


    public override void OnInstantiated()
    {
        popupMessageController = new(popupData, popupParent);
        dialogController = container.Instantiate<DialogController>();
        dialogController.Init(dialogData, dialogCanvas.transform);

        debugMessageController = container.Instantiate<DebugMessageController>();
    }

    public void ShowPopup(string message, string sender = default, PopupType popupType = PopupType.None) => popupMessageController.Show(message, sender, popupType);
    public void ShowDialog(Action<bool> callback, string message, string sender = default, DialogType dialogType = DialogType.OkCancel) => dialogController.ShowDialog(callback, message, sender, dialogType);

    private void OnDisable() => debugMessageController.Dispose();
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
