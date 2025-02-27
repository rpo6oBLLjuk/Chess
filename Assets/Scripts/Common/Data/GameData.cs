using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/GameData")]
public class GameData : ScriptableObject
{
    public AllowCaptures AllowCaptures = AllowCaptures.OpponentOnly;
    public bool allowRandomMove = false;
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