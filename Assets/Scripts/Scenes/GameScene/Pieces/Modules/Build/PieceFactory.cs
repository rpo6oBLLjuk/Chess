using System;
using UnityEngine;

[Serializable]
public class PieceFactory
{
    public PiecePrefabs PiecesPrefabs;

    public GameObject Get(PieceType pieceType) => PiecesPrefabs.Get(pieceType);
}
