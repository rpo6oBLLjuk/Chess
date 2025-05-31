using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Scriptable Objects/SkinData")]
public class SkinData : ScriptableObject
{
    public PiecesSkinData piecesSkinData;
    public CellsSkinData cellsSkinData;

    public PieceAnimationData pieceAnimationData;
    public CellAnimationData cellAnimationData;

    public PieceEffectData pieceEffectData;
    public CellEffectData cellEffectData;


    public void SetNewSkin(PiecesSkinData piecesSkinData, CellsSkinData cellsSkinData)
    {
        this.piecesSkinData = piecesSkinData;
        this.cellsSkinData = cellsSkinData;
    }
}
