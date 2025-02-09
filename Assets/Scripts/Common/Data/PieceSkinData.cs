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

    [SerializeField] private PieceSkinData Pawn;
    [SerializeField] private PieceSkinData Knight;
    [SerializeField] private PieceSkinData Bishop;
    [SerializeField] private PieceSkinData Rook;
    [SerializeField] private PieceSkinData Queen;
    [SerializeField] private PieceSkinData King;


    public Sprite Get(PieceType type, PieceColor color)
    {
        return type switch
        {
            PieceType.Pawn => (color == PieceColor.White) ? Pawn.WhiteSkin : Pawn.BlackSkin,
            PieceType.Knight => (color == PieceColor.White) ? Knight.WhiteSkin : Knight.BlackSkin,
            PieceType.Bishop => (color == PieceColor.White) ? Bishop.WhiteSkin : Bishop.BlackSkin,
            PieceType.Rook => (color == PieceColor.White) ? Rook.WhiteSkin : Rook.BlackSkin,
            PieceType.Queen => (color == PieceColor.White) ? Queen.WhiteSkin : Queen.BlackSkin,
            PieceType.King => (color == PieceColor.White) ? King.WhiteSkin : King.BlackSkin,
            _ => throw new NotImplementedException()
        };
    }
}
