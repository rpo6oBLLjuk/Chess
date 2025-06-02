using UnityEngine;

[CreateAssetMenu(fileName = "CellEffectData", menuName = "Scriptable Objects/Cell/CellEffectData")]
public class CellEffectData : ScriptableObject
{
    [field: SerializeField] public bool SelectInactiveCells { get; private set; } = false;
    [field: SerializeField] public bool DisablePreviousMoveCellsBeforeSelect { get; private set; } = false;
    //[field: MessageBox("Not working", MessageBoxType.Warning)]
    [field: SerializeField] public bool DisableCapturedCellsBeforeSelect { get; private set; } = false;
}
