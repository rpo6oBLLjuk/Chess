using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CellEffectManagerData", menuName = "Scriptable Objects/Cell/CellEffectManagerData")]
public class CellEffectManagerData : ScriptableObject
{
    [field: SerializeField] public bool SelectInactiveCells { get; private set; } = false;
    [field: SerializeField] public bool DisablePreviousMoveCellsBeforeSelect { get; private set; } = false;
    [field: MessageBox("Not working", MessageBoxType.Warning)]
    [field: SerializeField] public bool DisableCapturedCellsBeforeSelect { get; private set; } = false;
}
