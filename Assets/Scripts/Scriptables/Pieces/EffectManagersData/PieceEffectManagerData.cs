using Coffee.UIEffects;
using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceEffectManagerData", menuName = "Scriptable Objects/Piece/PieceEffectManagerData")]
public class PieceEffectManagerData : ScriptableObject
{
    [field: MessageBox("Need inject to CellEffectManager", MessageBoxType.Warning)]
    [field: SerializeField] public bool DragInactivePieces { get; private set; } = false;
}
