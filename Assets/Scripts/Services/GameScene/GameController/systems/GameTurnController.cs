using System;

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
    public bool CanChangeTurn = true;

    private PieceColor turnColor;

    public void PieceMoved()
    {
        if (CanChangeTurn)
            TurnColor = (TurnColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;
        else
            this.Log("State turn disabled");
    }
}
