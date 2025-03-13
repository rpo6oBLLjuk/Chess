using UnityEngine;
using Zenject;

public class PieceCapturer
{
    [Inject] GameController gameController;

    public void Init()
    {
    }

    public void CapturePiece(CellHandler cellHandler)
    {
        if (gameController.Board[cellHandler.Index] == 0)
            return;

        gameController.Board[cellHandler.Index] = 0;
        gameController.Pieces[cellHandler.Index] = null;

        cellHandler?.PieceRemoved();
    }
}