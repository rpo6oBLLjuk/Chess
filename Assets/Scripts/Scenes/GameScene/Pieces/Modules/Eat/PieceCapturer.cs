using Zenject;

public class PieceCapturer
{
    [Inject] GameController gameController;


    public void CapturePiece(CellHandler cellHandler)
    {
        if (cellHandler.CurrentPieceHandler == null)
            return;

        gameController.PiecesData.SetPiece(cellHandler.CellIndex, new PieceData());

        UnityEngine.Object.Destroy(cellHandler.CurrentPieceHandler.gameObject); //hard destroy
        cellHandler.PieceRemoved();
    }
}
