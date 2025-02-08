using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FigurePrefabs", menuName = "Scriptable Objects/Figure/Prefabs")]
public class FigurePrefabs : ScriptableObject
{
    [SerializeField] private GameObject Pawn;
    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Bishop;
    [SerializeField] private GameObject Rook;
    [SerializeField] private GameObject Queen;
    [SerializeField] private GameObject King;


    public GameObject Get(FigureType type)
    {
        return type switch
        {
            FigureType.Pawn => Pawn,
            FigureType.Knight => Knight,
            FigureType.Bishop => Bishop,
            FigureType.Rook => Rook,
            FigureType.Queen => Queen,
            FigureType.King => King,
            _ => throw new NotImplementedException()
        };
    }
}
