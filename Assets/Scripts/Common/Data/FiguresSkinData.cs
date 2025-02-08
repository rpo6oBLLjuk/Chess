using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FigureSkinData", menuName = "Scriptable Objects/Figure/SkinData")]
public class FiguresSkinData : ScriptableObject
{
    [Serializable]
    private class FigureSkinData
    {
        public Sprite WhiteSkin;
        public Sprite BlackSkin;
    }

    [SerializeField] private FigureSkinData Pawn;
    [SerializeField] private FigureSkinData Knight;
    [SerializeField] private FigureSkinData Bishop;
    [SerializeField] private FigureSkinData Rook;
    [SerializeField] private FigureSkinData Queen;
    [SerializeField] private FigureSkinData King;


    public Sprite Get(FigureType type, FigureColor color)
    {
        return type switch
        {
            FigureType.Pawn => (color == FigureColor.White) ? Pawn.WhiteSkin : Pawn.BlackSkin,
            FigureType.Knight => (color == FigureColor.White) ? Knight.WhiteSkin : Knight.BlackSkin,
            FigureType.Bishop => (color == FigureColor.White) ? Bishop.WhiteSkin : Bishop.BlackSkin,
            FigureType.Rook => (color == FigureColor.White) ? Rook.WhiteSkin : Rook.BlackSkin,
            FigureType.Queen => (color == FigureColor.White) ? Queen.WhiteSkin : Queen.BlackSkin,
            FigureType.King => (color == FigureColor.White) ? King.WhiteSkin : King.BlackSkin,
            _ => throw new NotImplementedException()
        };
    }
}
