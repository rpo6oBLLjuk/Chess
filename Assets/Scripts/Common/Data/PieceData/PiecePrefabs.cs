using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PiecePrefabs", menuName = "Scriptable Objects/Piece/Prefabs")]
public class PiecePrefabs : ScriptableObject
{
    [SerializeField] private GameObject Pawn;
    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Bishop;
    [SerializeField] private GameObject Rook;
    [SerializeField] private GameObject Queen;
    [SerializeField] private GameObject King;


    public GameObject Get(PieceType type)
    {
        return type switch
        {
            PieceType.Pawn => Pawn,
            PieceType.Knight => Knight,
            PieceType.Bishop => Bishop,
            PieceType.Rook => Rook,
            PieceType.Queen => Queen,
            PieceType.King => King,
            _ => throw new NotImplementedException()
        };
    }
}
