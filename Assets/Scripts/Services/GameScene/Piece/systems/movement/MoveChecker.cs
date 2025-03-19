using Zenject;

public class MoveChecker
{
    [Inject] GameManager gameManager;


    public bool IsMoveAllowed(byte startIndex, byte endIndex)
    {
        foreach (byte moveIndex in gameManager.Moves[startIndex])
        {
            if (moveIndex == endIndex)
            {
                if (PiecePacker.IsDefaultPiece(gameManager.Board[endIndex]))
                    gameManager.CapturePiece(gameManager.Pieces[startIndex], gameManager.Cells[endIndex]);
                return true;
            }
        }
        return false;
    }

    public bool IsMoveValid(byte startIndex, byte endIndex) => IsMoveValid(gameManager.Board[startIndex], gameManager.Board[endIndex], startIndex, endIndex);
    public bool IsMoveValid(byte movingPieceData, byte endPieceData, byte startIndex, byte endIndex)
    {
        if (!IsMovementAllowed(movingPieceData, startIndex, endIndex)) //Выход если мувмент заблокирован
            return false;

        if (PiecePacker.IsEqualType(endPieceData, PieceType.None)) //Выход если в конечной точке пустая клетка
            return true;

        if (IsCaptureAllowed(movingPieceData, endPieceData)) //Выход если фигуру в конечной точке можно съесть
            return true;

        return false; //Выход если в конечной точке есть фигура, которую не удалось съесть
    }

    public bool IsMovementAllowed(byte movingPieceData, byte startIndex, byte endIndex)
    {
        if (gameManager.GameData.AllowMovement == AllowMovement.None)
            return false;

        //if (gameManager.GameData.AllowMovement == AllowMovement.All)
        //    return true;
        //Здесь больше нет проверки корректности мува, => AllowMovement.Default тоже должен вернуть true
        return true;
    }

    public bool IsCaptureAllowed(byte movingPieceData, byte endPieceData)
    {
        if (gameManager.GameData.AllowCaptures == AllowCapture.None && !PiecePacker.IsEqualType(endPieceData, PieceType.None))
            return false;

        if (gameManager.GameData.AllowCaptures == AllowCapture.All)
            return true;


        if (PiecePacker.IsEqualType(endPieceData, PieceType.None))
            return true;
        if (PiecePacker.IsEqualType(endPieceData, PieceType.Other))
            return false;

        if (!PiecePacker.IsEqualColor(movingPieceData, endPieceData))
            return true;
        else
            return false;
    }
}
