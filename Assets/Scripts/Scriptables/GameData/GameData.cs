using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/GameData")]
public class GameData : ScriptableObject
{
    public Action DataChanged;

    [field: SerializeField] public AllowCapture AllowCaptures { get; private set; }
    [field: SerializeField] public AllowMovement AllowMovement { get; set; }


    private void OnValidate() => DataChanged?.Invoke();
}