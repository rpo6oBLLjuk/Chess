using System;
using UnityEngine;

[Serializable]
public class PieceFactory : AbstractFactory
{
    public PiecePrefabs PiecesPrefabs;

    public GameObject Get(PieceType type) => Get(PiecesPrefabs.Get(type));
}
