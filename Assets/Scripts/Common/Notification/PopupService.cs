using System;
using UnityEngine;

public class PopupService : MonoBehaviour
{
    public PopupData popupData;
    public Transform popupParent;

    private PopupMessageController popupMessageController;


    private void Awake() => popupMessageController = new PopupMessageController(popupData, popupParent);

    public void Show(string message, string sender = default, PopupType popupType = PopupType.None) => popupMessageController.Show(message, sender, popupType);
}

public enum PopupType
{
    None,
    Info,
    Warning,
    Error
}
