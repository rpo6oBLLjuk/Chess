using Zenject;

public class PieceDataMover
{
    [Inject] GameManager gameManager;


    public void Move(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
        startCell.PieceRemoved();
        endCell.PiecePlaced(pieceHandler);

        gameManager.Moves.Add(new Move(gameManager.Board[startCell.Index], startCell.Index, endCell.Index));
        MovePieceData(startCell.Index, endCell.Index);
    }

    private void MovePieceData(byte startIndex, byte endIndex)
    {
        (gameManager.Board[startIndex], gameManager.Board[endIndex]) = (gameManager.Board[endIndex], gameManager.Board[startIndex]);
        (gameManager.Pieces[startIndex], gameManager.Pieces[endIndex]) = (gameManager.Pieces[endIndex], gameManager.Pieces[startIndex]);
    }
}
