using Zenject;

public class PieceDestroyer
{
    [Inject] GameController gameController;

    public void DestroyPiece(CellHandler cellHandler)
    {
        if (cellHandler.CurrentPieceHandler == null)
            return;

        gameController.PiecesData.Set(cellHandler.CellIndex, new PieceData());
        UnityEngine.Object.Destroy(cellHandler.CurrentPieceHandler.gameObject); //hard destroy
    }
}
