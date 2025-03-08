using Zenject;

public class PieceCapturer
{
    [Inject] GameController gameController;


    public void CapturePiece(CellHandler cellHandler)
    {
        if (cellHandler.CurrentPieceHandler == null)
            return;

        gameController.Pieces[cellHandler.CellIndex] = 0;

        UnityEngine.Object.Destroy(cellHandler.CurrentPieceHandler.gameObject); //hard destroy
        cellHandler.PieceRemoved();
    }
}
