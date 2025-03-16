using Zenject;

public class PieceCapturer
{
    [Inject] GameManager gameManager;

    public void Init()
    {
    }

    public void CapturePiece(CellHandler cellHandler)
    {
        if (PiecePacker.IsEqualType(ref gameManager.Board[cellHandler.Index], PieceType.None))
            return;

        gameManager.Board[cellHandler.Index] = 0;
        gameManager.Pieces[cellHandler.Index] = null;

        cellHandler?.PieceRemoved();
    }
}