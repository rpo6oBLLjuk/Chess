using UnityEngine;

[CreateAssetMenu(fileName = "PieceEffectData", menuName = "Scriptable Objects/Piece/PieceEffectData")]
public class PieceEffectData : ScriptableObject
{
    [field: SerializeField] public bool DragInactivePieces { get; private set; } = false;
}
