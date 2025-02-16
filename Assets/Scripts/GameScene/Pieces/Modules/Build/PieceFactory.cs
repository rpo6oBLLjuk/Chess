using System;
using UnityEngine;

[Serializable]
public class PieceFactory : AbstractFactory
{
    public PiecePrefabs PiecesPrefabs;

    public GameObject Get(PieceType type) => PiecesPrefabs.Get(type);
}
