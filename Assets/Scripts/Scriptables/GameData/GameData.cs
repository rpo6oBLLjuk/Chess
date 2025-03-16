using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/GameData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public AllowCapture AllowCaptures { get; private set; }
    [field: SerializeField] public AllowMovement AllowMovement { get; set; }
}