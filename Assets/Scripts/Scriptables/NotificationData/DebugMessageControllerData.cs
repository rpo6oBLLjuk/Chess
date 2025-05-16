using UnityEngine;

[CreateAssetMenu(fileName = "DebugMessage", menuName = "Scriptable Objects/Notification/DebugMessageData")]
public class DebugMessageControllerData : ScriptableObject
{
    public bool LogInfo = false;
    public bool LogWarning = false;
    public bool LogError = true;
}
