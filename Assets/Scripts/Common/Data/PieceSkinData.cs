using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceSkinData", menuName = "Scriptable Objects/Piece/SkinData")]
public class PiecesSkinData : ScriptableObject
{
    [Serializable]
    private class PieceSkinData
    {
        public Sprite WhiteSkin;
        public Sprite BlackSkin;
    }

    [field: SerializeField] public PieceAnimationData AnimationData { get; private set; }

    [SerializeField] private PieceSkinData Pawn;
    [SerializeField] private PieceSkinData Knight;
    [SerializeField] private PieceSkinData Bishop;
    [SerializeField] private PieceSkinData Rook;
    [SerializeField] private PieceSkinData Queen;
    [SerializeField] private PieceSkinData King;


    public Sprite Get(PieceData data)
    {
        return data.Type switch
        {
            PieceType.Pawn => (data.Color == PieceColor.White) ? Pawn.WhiteSkin : Pawn.BlackSkin,
            PieceType.Knight => (data.Color == PieceColor.White) ? Knight.WhiteSkin : Knight.BlackSkin,
            PieceType.Bishop => (data.Color == PieceColor.White) ? Bishop.WhiteSkin : Bishop.BlackSkin,
            PieceType.Rook => (data.Color == PieceColor.White) ? Rook.WhiteSkin : Rook.BlackSkin,
            PieceType.Queen => (data.Color == PieceColor.White) ? Queen.WhiteSkin : Queen.BlackSkin,
            PieceType.King => (data.Color == PieceColor.White) ? King.WhiteSkin : King.BlackSkin,
            _ => throw new NotImplementedException()
        };
    }
}
