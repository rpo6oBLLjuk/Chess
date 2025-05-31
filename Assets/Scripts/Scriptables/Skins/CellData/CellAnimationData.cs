using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CellAnimationData", menuName = "Scriptable Objects/Cell/AnimationData")]
public class CellAnimationData : ScriptableObject
{
    [Serializable]
    public class AnimationData
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.5f;
    }

    [field: SerializeField] public AnimationData TargetAnimationData { get; private set; }
    [field: SerializeField] public AnimationData SelectAnimationData { get; private set; }
    [field: SerializeField] public AnimationData PossibleMoveAnimationData { get; private set; }
    [field: SerializeField] public AnimationData CaptureAnimationData { get; private set; }
    [field: SerializeField] public AnimationData PreviousMoveAnimationData { get; private set; }
    [field: SerializeField] public AnimationData HoverAnimationData { get; private set; }
}
