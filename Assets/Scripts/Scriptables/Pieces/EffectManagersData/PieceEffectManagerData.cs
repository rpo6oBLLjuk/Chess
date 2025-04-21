using UnityEngine;

[CreateAssetMenu(fileName = "PieceEffectManagerData", menuName = "Scriptable Objects/Piece/PieceEffectManagerData")]
public class PieceEffectManagerData : ScriptableObject
{
    [field: SerializeField] public bool DragInactivePieces { get; private set; } = false;
}
