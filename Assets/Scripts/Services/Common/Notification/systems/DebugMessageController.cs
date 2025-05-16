using System;
using UnityEngine;
using Zenject;

public class DebugMessageController : IDisposable
{
    [Inject] NotificationService notificationService;


    public DebugMessageController()
    {
        Application.logMessageReceived += HandleLog;
    }

    public void Dispose()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        PopupType logType = type switch
        {
            LogType.Log => PopupType.Info,
            LogType.Warning => PopupType.Warning,
            LogType.Exception => PopupType.Error,
            LogType.Assert => PopupType.Error,
            _ => PopupType.Info
        };

        if (logType switch
        {
            PopupType.Info => notificationService.debugMessageControllerData.LogInfo,
            PopupType.Warning => notificationService.debugMessageControllerData.LogWarning,
            PopupType.Error => notificationService.debugMessageControllerData.LogError,
            _ => false
        })
            notificationService.ShowPopup(logString, "Log Message", logType);
    }
}
