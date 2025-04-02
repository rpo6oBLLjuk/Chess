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
    private PieceColor turnColor;


    public void PieceMoved() => TurnColor = TurnColor.Invert();
}
