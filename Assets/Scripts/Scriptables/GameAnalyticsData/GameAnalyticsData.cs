using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAnalyticsData", menuName = "Scriptable Objects/GameAnalyticsData")]
public class GameAnalyticsData : ScriptableObject
{
    [SerializableDateTime(SerializableDateTime.InspectorFormat.AddTextInput)]
    public SerializableDateTime registrationDate;
    [SerializableDateTime(SerializableDateTime.InspectorFormat.AddTextInput)]
    public SerializableDateTime lastLoginDate;

    public int winsCount;
    public int defeatsCount;
}
