using Zenject;

public class MoveChecker
{
    [Inject] GameManager gameManager;


    public bool CanBeMove(CellHandler startCell, CellHandler endCell)
    {
        byte endPieceData = gameManager.Board[endCell.Index];
        byte movedPieceData = gameManager.Board[startCell.Index];

        byte startIndex = startCell.Index;
        byte endIndex = endCell.Index;

        return CanBeMove(ref movedPieceData, ref endPieceData, ref startIndex, ref endIndex);
    }

    public bool CanBeMove(ref byte movingPieceData, ref byte endPieceData, ref byte startIndex, ref byte endIndex)
    {
        if (!CheckMoveByPiece(ref movingPieceData, ref startIndex, ref endIndex)) //Выход если мувмент заблокирован
            return false;

        if (PiecePacker.IsEqualType(ref endPieceData, PieceType.None)) //Выход если в конечной точке пустая клетка
            return true;

        if (CheckCapture(ref movingPieceData, ref endPieceData)) //Выход если фигуру в конечной точке можно съесть
        {
            gameManager.CapturePiece(gameManager.Pieces[startIndex], gameManager.Cells[endIndex]);
            return true;
        }
        return false; //Выход если в конечной точке есть фигура, которую не удалось съесть
    }

    private bool CheckMoveByPiece(ref byte movingPieceData, ref byte startIndex, ref byte endIndex)
    {
        if (gameManager.GameData.AllowMovement == AllowMovement.None)
            return false;

        if (gameManager.GameData.AllowMovement == AllowMovement.All)
            return true;



        return true;
    }

    private bool CheckCapture(ref byte movingPieceData, ref byte endPieceData)
    {
        if (gameManager.GameData.AllowCaptures == AllowCapture.None)
            return false;

        if (gameManager.GameData.AllowCaptures == AllowCapture.All)
            return true;


        if (PiecePacker.IsEqualType(ref endPieceData, PieceType.None))
            return true;
        if (PiecePacker.IsEqualType(ref endPieceData, PieceType.Other))
            return false;

        if (!PiecePacker.IsEqualColor(ref movingPieceData, ref endPieceData))
            return true;
        else
            return false;
    }
}
