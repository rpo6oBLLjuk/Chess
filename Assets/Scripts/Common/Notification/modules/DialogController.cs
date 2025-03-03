using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController
{
    private DialogData data;

    private Transform parentCanvas;

    private GameObject currentDialog;
    private AnimatedPanel currectDialogPanel;


    public DialogController(DialogData data, Transform parent)
    {
        this.data = data;
        this.parentCanvas = parent;
    }

    public void ShowDialog(Action<bool> callback, string message, string sender = default, DialogType dialogType = DialogType.OkCancel)
    {
        currentDialog = InstantiateDialod(dialogType);
        ConfigurateDialog(currentDialog, callback, message, sender, dialogType);

        currectDialogPanel = currentDialog.GetComponentInChildren<AnimatedPanel>();
        currectDialogPanel.Show();
    }

    private void ConfigurateDialog(GameObject dialog, Action<bool> callback, string message, string sender = default, DialogType dialogType = DialogType.OkCancel)
    {
        dialog.transform.FindDeepChild("DialogText").GetComponent<TextMeshProUGUI>().text = message;
        dialog.transform.FindDeepChild("DialogSender").GetComponent<TextMeshProUGUI>().text = sender;

        if (dialogType == DialogType.OkCancel)
        {
            DebugExtensions.Log(message, sender);

            dialog.transform.FindDeepChild("OkButton").GetComponent<Button>().onClick.AddListener(() => OkButtonListener(callback, message, sender));
            dialog.transform.FindDeepChild("CancelButton").GetComponent<Button>().onClick.AddListener(() => CancelButtonListener(callback, message, sender));
        }
    }

    private void OkButtonListener(Action<bool> callback, string message, string sender)
    {
        DebugExtensions.Log($"Dialog \"{message}\" CONFIRMED", sender);

        callback?.Invoke(true);
        currectDialogPanel.GetComponentInChildren<AnimatedPanel>().Hide();
    }
    private void CancelButtonListener(Action<bool> callback, string message, string sender)
    {
        DebugExtensions.Log($"Dialog \"{message}\" CANCELED", sender);

        callback?.Invoke(false);
        currectDialogPanel.GetComponentInChildren<AnimatedPanel>().Hide();
    }


    private GameObject InstantiateDialod(DialogType dialogType) => UnityEngine.Object.Instantiate(GetDialogByType(dialogType), parentCanvas);
    private GameObject GetDialogByType(DialogType dialogType) => dialogType switch
    {
        DialogType.OkCancel => data.OkCancelDialog,
        _ => throw new System.NotImplementedException()
    };
}
