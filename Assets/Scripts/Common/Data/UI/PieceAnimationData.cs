using UnityEngine;

[CreateAssetMenu(fileName = "PieceAnimationData", menuName = "Scriptable Objects/Piece/AnimationData")]
public class PieceAnimationData : ScriptableObject
{
    public float delayBeforeHadnle = 0.3f;

    public float scaleMultiplier = 1.5f;
    public float scaleDuration = 0.25f;

    public float magnetToCellDuration = 0.1f;
    [Tooltip("Scaled by Time.deltaTime")]
    public float magnetToMouseLerpValue = 15f;
}
