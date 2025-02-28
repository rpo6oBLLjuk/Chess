using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CellsSkinData", menuName = "Scriptable Objects/Cell/SkinData")]
public class CellsSkinData : ScriptableObject
{
    [Serializable]
    public class CellSpriteData
    {
        public Sprite Sprite;
        public Color Color = Color.white;
    }

    public Sprite WhiteCell;
    public Sprite BlackCell;

    [Space]
    public CellSpriteData TargetImage;
    public CellSpriteData SelectImage;
    public CellSpriteData PossibleMoveImage;
    public CellSpriteData CaptureImage;
    public CellSpriteData LastMoveImage;
}
