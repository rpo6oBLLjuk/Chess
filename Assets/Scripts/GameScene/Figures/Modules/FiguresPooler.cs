using System;
using UnityEngine;

[Serializable]
public class FiguresPooler : AbstractFactory
{
    public FigurePrefabs figuresPrefabs;


    public GameObject Get(FigureType type) => Get(figuresPrefabs.Get(type));
}
