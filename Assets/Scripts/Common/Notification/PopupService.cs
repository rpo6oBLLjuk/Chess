using System;
using UnityEngine;
using Zenject;

public class PopupService : MonoService
{
    public PopupData popupData;
    public Transform popupParent;

    private PopupMessageController popupMessageController;


    public override void OnInstantiated() => popupMessageController = new PopupMessageController(popupData, popupParent);

    public void Show(string message, string sender = default, PopupType popupType = PopupType.None) => popupMessageController.Show(message, sender, popupType);
}

public enum PopupType
{
    None,
    Info,
    Warning,
    Error
}
