using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CellAnimationData", menuName = "Scriptable Objects/Cell/AnimationData")]
public class CellAnimationData : ScriptableObject
{
    [Serializable]
    public class AnimationData
    {
        public float ShowTime = 0.1f;
        public float HideTime = 0.1f;
    }

    public AnimationData TargetAnimationData;
    public AnimationData SelectAnimationData;
    public AnimationData PossibleMoveAnimationData;
    public AnimationData CaptureAnimationData;
    public AnimationData LastMoveAnimationData;
}
