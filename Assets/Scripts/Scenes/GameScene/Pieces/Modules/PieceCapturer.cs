using Zenject;

public class PieceCapturer
{
    [Inject] GameController gameController;


    public void CapturePiece(CellHandler cellHandler)
    {
        if (gameController.Board[cellHandler.Index] == 0)
            return;

        gameController.Board[cellHandler.Index] = 0;

        UnityEngine.Object.Destroy(gameController.Pieces[cellHandler.Index].gameObject); //hard destroy
        gameController.Pieces[cellHandler.Index] = null;

        cellHandler.PieceRemoved();
    }
}
