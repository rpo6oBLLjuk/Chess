using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FigureObjectsData", menuName = "Scriptable Objects/FigureObjectsData")]
public class FigureObjectsData : ScriptableObject
{
    [Serializable]
    private class FigureSkinData
    {
        public GameObject figure;
        public SpriteRenderer WhiteSkin;
        public SpriteRenderer BlackSkin;
    }

    [SerializeField] private FigureSkinData Pawn;
    [SerializeField] private FigureSkinData Knight;
    [SerializeField] private FigureSkinData Bishop;
    [SerializeField] private FigureSkinData Rook;
    [SerializeField] private FigureSkinData Queen;
    [SerializeField] private FigureSkinData King;


    public GameObject GetFigureByType(FigureData data)
    {
        return data.Type switch
        {
            FigureType.Pawn => Pawn.figure,
            FigureType.Knight => Knight.figure,
            FigureType.Bishop => Bishop.figure,
            FigureType.Rook => Rook.figure,
            FigureType.Queen => Queen.figure,
            FigureType.King => King.figure,
            _ => null
        };
    }
}
