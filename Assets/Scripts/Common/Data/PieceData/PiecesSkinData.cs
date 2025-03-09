using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PiecesSkinData", menuName = "Scriptable Objects/Piece/SkinData")]
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


    public Sprite Get(byte pieceData)
    {
        PieceColor pieceColor = PiecePacker.GetColor(pieceData);
        return PiecePacker.GetType(pieceData) switch
        {
            PieceType.Pawn => (pieceColor == PieceColor.White) ? Pawn.WhiteSkin : Pawn.BlackSkin,
            PieceType.Knight => (pieceColor == PieceColor.White) ? Knight.WhiteSkin : Knight.BlackSkin,
            PieceType.Bishop => (pieceColor == PieceColor.White) ? Bishop.WhiteSkin : Bishop.BlackSkin,
            PieceType.Rook => (pieceColor == PieceColor.White) ? Rook.WhiteSkin : Rook.BlackSkin,
            PieceType.Queen => (pieceColor == PieceColor.White) ? Queen.WhiteSkin : Queen.BlackSkin,
            PieceType.King => (pieceColor == PieceColor.White) ? King.WhiteSkin : King.BlackSkin,
            _ => throw new NotImplementedException()
        };
    }
}
