using System;
using UnityEngine;

public class CellHandler : MonoBehaviour
{
    public Action<Vector2Int, GameObject> OnFigurePlaced;

    public Vector2Int cellIndex;


    public void Init(int x, int y)
    {
        cellIndex = new Vector2Int(x, y);
    }

    public void FigurePlaced(GameObject figure)
    {
        OnFigurePlaced?.Invoke(cellIndex, figure);
    }
}
