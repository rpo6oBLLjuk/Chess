using UnityEngine;

[CreateAssetMenu(fileName = "PieceAnimationData", menuName = "Scriptable Objects/Piece/AnimationData")]
public class PieceAnimationData : ScriptableObject
{
    public float delayBeforeHadnle = 0.3f;

    public float scaleMultiplier = 1.5f;
    public float scaleDuration = 0.25f;

    public float magnetDuration = 0.1f;
}
