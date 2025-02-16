using UnityEngine;

public static class DebugExtensions
{
    public static void Log(string message, string sender) => Debug.Log($"[{sender}] {message}");
    public static void LogWarning(string message, string sender) => Debug.LogWarning($"[{sender}] {message}");
    public static void LogError(string message, string sender) => Debug.LogError($"[{sender}] {message}");
}
