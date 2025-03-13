using Zenject;

public class MoveChecker
{
    [Inject] GameController gameController;


    public bool CanBeMove(CellHandler startCell, CellHandler endCell)
    {
        byte endPieceData = gameController.Board[endCell.Index];
        if (endPieceData == 0)
            return true;


        PiecePacker.GetType(ref endPieceData, out PieceType endPieceType);
        switch (endPieceType)
        {
            case PieceType.None:
            return true;
            case PieceType.Other:
            return false;
            default:
            {
                if (PiecePacker.GetColor(endPieceData) != PiecePacker.GetColor(gameController.Board[startCell.Index]))
                {
                    gameController.CapturePiece(gameController.Pieces[startCell.Index], endCell);
                    return true;
                }
                else
                    return false;
            }
        }
    }

    public bool CanBeMove(byte movingPiece, byte startIndex, byte endIndex)
    {
        return true;
    }
}
