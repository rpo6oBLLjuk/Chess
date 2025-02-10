using System;
using UnityEngine;

public class PopupService : MonoBehaviour
{
    public PopupData popupData;
    public Transform popupParent;

    private PopupMessageController popupMessageController;


    private void Awake() => popupMessageController = new PopupMessageController(popupData, popupParent);

    public void Show(string message, PopupType popupType = PopupType.None) => popupMessageController.Show(message, popupType);
}

public enum PopupType
{
    None,
    Info,
    Warning,
    Error
}
