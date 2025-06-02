using ModestTree;
using System.Linq;
using Zenject;

public class PieceCapturer
{
    [Inject] GameManager gameManager;

    public void Init()
    {
    }

    public void CapturePiece(PieceHandler capturer, CellHandler endCellHandler)
    {
        if (PiecePacker.IsEqualType(gameManager.Board[endCellHandler.Index], PieceType.None))
            return;

        gameManager.Captures.Add(new Capture(
            (byte)gameManager.Moves.Count(),
            capturer != null ? gameManager.Board[gameManager.Pieces.IndexOf(capturer)] : (byte)0,
            gameManager.Board[endCellHandler.Index]));

        gameManager.Board[endCellHandler.Index] = 0;
        gameManager.Pieces[endCellHandler.Index] = null;

        endCellHandler?.PieceRemoved();
    }

    public void DestroyPiece(CellHandler endCellHandler)
    {
        if (PiecePacker.IsEqualType(gameManager.Board[endCellHandler.Index], PieceType.None))
            return;

        gameManager.Captures.Add(new Capture(
            (byte)gameManager.Moves.Count(),
            0,
            gameManager.Board[endCellHandler.Index]));

        gameManager.Board[endCellHandler.Index] = 0;
        gameManager.Pieces[endCellHandler.Index] = null;

        endCellHandler?.PieceRemoved();
    }
}