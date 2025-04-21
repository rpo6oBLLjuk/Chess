using UnityEngine;

[CreateAssetMenu(fileName = "CellEffectManagerData", menuName = "Scriptable Objects/Cell/CellEffectManagerData")]
public class CellEffectManagerData : ScriptableObject
{
    [field: SerializeField] public bool SelectInactiveCells { get; private set; } = false;
}
