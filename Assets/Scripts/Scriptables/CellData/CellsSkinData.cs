using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CellsSkinData", menuName = "Scriptable Objects/Cell/SkinData")]
public class CellsSkinData : ScriptableObject
{
    [Serializable]
    public class CellSpriteData
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }

    [field: SerializeField]
    public CellAnimationData AnimationData { get; private set; }

    [field: SerializeField] public Sprite WhiteCell { get; private set; }
    [field: SerializeField] public Sprite BlackCell { get; private set; }

    [field: Space]
    [field: SerializeField] public CellSpriteData TargetCellData { get; private set; }
    [field: SerializeField] public CellSpriteData SelectCellData { get; private set; }
    [field: SerializeField] public CellSpriteData PossibleMoveCellData { get; private set; }
    [field: SerializeField] public CellSpriteData CaptureCellData { get; private set; }
    [field: SerializeField] public CellSpriteData PreviousMoveCellData { get; private set; }
    [field: SerializeField] public CellSpriteData HoverCellData { get; private set; }
}
