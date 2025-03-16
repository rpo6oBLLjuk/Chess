using Zenject;

public class PieceMover
{
    [Inject] GameManager gameManager;


    public void Move(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        startCell.PieceRemoved();
        endCell.PiecePlaced(pieceHandler);

        MovePieceData(startCell.Index, endCell.Index);
    }

    private void MovePieceData(byte startIndex, byte endIndex)
    {
        (gameManager.Board[startIndex], gameManager.Board[endIndex]) = (gameManager.Board[endIndex], gameManager.Board[startIndex]);
        (gameManager.Pieces[startIndex], gameManager.Pieces[endIndex]) = (gameManager.Pieces[endIndex], gameManager.Pieces[startIndex]);
    }
}
