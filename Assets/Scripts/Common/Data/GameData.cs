using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/GameData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public AllowCaptures AllowCaptures { get; private set; }
    [field: SerializeField] public bool AlowProhibitedMovements { get; set; }
}

public enum GameState
{
    none,
    gameStarted,
    gameFinished
}

public enum AllowCaptures
{
    None,
    OpponentOnly,
    All
}