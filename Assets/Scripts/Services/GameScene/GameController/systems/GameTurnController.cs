using System;
using UnityEngine;

[Serializable]
public class GameTurnController
{
    public Action<PieceColor> OnTurnChanged;

    public PieceColor TurnColor
    {
        get => turnColor;
        private set
        {
            turnColor = value;
            OnTurnChanged?.Invoke(turnColor);
        }
    }
    public bool UseTurn = true;

    [SerializeField] private PieceColor turnColor;
    [SerializeField] private bool logging = false;


    public void PieceMoved()
    {
        if (UseTurn)
            TurnColor = TurnColor.Invert();
        else if (logging)
            this.Log("State turn disabled");
    }
}
