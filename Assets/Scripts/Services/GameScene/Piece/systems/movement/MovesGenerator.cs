using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MovesGenerator
{
    [Inject] GameManager gameManager;
    PieceService pieceService;

    private byte[] fantomBoard;
    private List<byte>[] fantomMoves;

    public void Init(PieceService pieceService) => this.pieceService = pieceService;

    public void GenerateAllPossibleMoves()
    {
        gameManager.Moves = new List<byte>[64];

        for (byte index = 0; index < 64; index++)
        {
            if (PiecePacker.IsDefaultPiece(gameManager.Board[index]))
            {
                gameManager.Moves[index] = new();
                GeneratePossibleMovesForPiece(index);
            }
        }
    }

    private void GeneratePossibleMovesForPiece(byte index)
    {
        if (gameManager.GameData.AllowMovement == AllowMovement.All)
        {
            gameManager.Moves[index] = Enumerable.Range(0, 64)
                .Where(x => pieceService.MoveChecker.IsCaptureAllowed(gameManager.Board[index], gameManager.Board[x]))
                .Select(x => (byte)x)
                .ToList();
            return;
        }

        PiecePacker.GetColor(gameManager.Board[index], out PieceColor pieceColor);

        switch (PiecePacker.GetType(gameManager.Board[index]))
        {
            case PieceType.Pawn:
                GeneratePawnMoves(index, pieceColor);
                break;
            case PieceType.Knight:
                GenerateKnightMoves(index, pieceColor);
                break;
            case PieceType.Bishop:
                GenerateBishopMoves(index, pieceColor);
                break;
            case PieceType.Rook:
                GenerateRookMoves(index, pieceColor);
                break;
            case PieceType.Queen:
                GenerateQueenMoves(index, pieceColor);
                break;
            case PieceType.King:
                GenerateKingMoves(index, pieceColor);
                break;
        }
    }

    private void GeneratePawnMoves(byte index, PieceColor pieceColor)
    {
        if (pieceColor == PieceColor.White)
        {
            if (OnTop(index))
            {
                this.LogWarning("Pawn_White on the top row");
                return;
            }

            if (pieceService.MoveChecker.IsMovementAllowed(gameManager.Board[index], index, (byte)(index - 8)) && PiecePacker.IsEqualType(gameManager.Board[index - 8], PieceType.None))
            {
                gameManager.Moves[index].Add((byte)(index - 8));
                if (index >= 48 && index <= 55 && pieceService.MoveChecker.IsMovementAllowed(gameManager.Board[index], index, (byte)(index - 16)) && PiecePacker.IsEqualType(gameManager.Board[index - 16], PieceType.None))
                {
                    gameManager.Moves[index].Add((byte)(index - 16));
                }
            }

            if (!OnLeft(index))
            {
                if (PiecePacker.IsDefaultPiece(gameManager.Board[index - 9]))
                    ApplyMove(index, (byte)(index - 9));
            }

            if (!OnRight(index))
            {
                if (PiecePacker.IsDefaultPiece(gameManager.Board[index - 7]))
                    ApplyMove(index, (byte)(index - 7));
            }
        }
        else //if (pieceColor == PieceColor.Black)
        {
            if (OnBottom(index))
            {
                this.LogWarning("Pawn_Black on the bottom row");
                return;
            }

            if (pieceService.MoveChecker.IsMovementAllowed(gameManager.Board[index], index, (byte)(index + 8)) && PiecePacker.IsEqualType(gameManager.Board[index + 8], PieceType.None))
            {
                gameManager.Moves[index].Add((byte)(index + 8));

                if (index >= 8 && index <= 17 && pieceService.MoveChecker.IsMovementAllowed(gameManager.Board[index], index, (byte)(index + 16)) && PiecePacker.IsEqualType(gameManager.Board[index + 16], PieceType.None))
                {
                    gameManager.Moves[index].Add((byte)(index + 16));
                }
            }

            if (!OnLeft(index))
            {
                if (PiecePacker.IsDefaultPiece(gameManager.Board[index + 9]))
                    ApplyMove(index, (byte)(index + 9));
            }

            if (!OnRight(index))
            {
                if (PiecePacker.IsDefaultPiece(gameManager.Board[index + 7]))
                    ApplyMove(index, (byte)(index + 7));
            }
        }
    }

    private void GenerateKnightMoves(byte index, PieceColor pieceColor)
    {
        if (!OnLeft(index))
        {
            if (index >= 16)
                ApplyMove(index, (byte)(index - 8 - 8 - 1));

            if (index <= 47)
                ApplyMove(index, (byte)(index + 8 + 8 - 1));
        }
        if (!OnRight(index))
        {
            if (index >= 16)
                ApplyMove(index, (byte)(index - 8 - 8 + 1));

            if (index <= 47)
                ApplyMove(index, (byte)(index + 8 + 8 + 1));
        }

        if (!OnTop(index))
        {
            if (index % 8 > 1)
                ApplyMove(index, (byte)(index - 1 - 1 - 8));

            if (index % 8 < 6)
                ApplyMove(index, (byte)(index + 1 + 1 - 8));
        }
        if (!OnBottom(index))
        {
            if (index % 8 > 1)
                ApplyMove(index, (byte)(index - 1 - 1 + 8));

            if (index % 8 < 6)
                ApplyMove(index, (byte)(index + 1 + 1 + 8));
        }
    }
    private void GenerateKingMoves(byte index, PieceColor pieceColor)
    {
        bool onLeft = OnLeft(index);
        bool onRight = OnRight(index);
        bool onTop = OnTop(index);
        bool onBottom = OnBottom(index);

        if (!onTop)
        {
            if (!onLeft)
                ApplyMove(index, (byte)(index - 9));

            if (!onRight)
                ApplyMove(index, (byte)(index - 7));

            ApplyMove(index, (byte)(index - 8));
        }
        if (!onBottom)
        {
            if (!onLeft)
                ApplyMove(index, (byte)(index + 7));

            if (!onRight)
                ApplyMove(index, (byte)(index + 9));

            ApplyMove(index, (byte)(index + 8));
        }

        if (!onLeft)
            ApplyMove(index, (byte)(index - 1));
        if (!onRight)
            ApplyMove(index, (byte)(index + 1));
    }

    private void GenerateBishopMoves(byte index, PieceColor pieceColor) => GenerateDiagonalMove(index, pieceColor);
    private void GenerateRookMoves(byte index, PieceColor pieceColor) => GenerateOrthogonalMove(index, pieceColor);
    private void GenerateQueenMoves(byte index, PieceColor pieceColor)
    {
        GenerateDiagonalMove(index, pieceColor);
        GenerateOrthogonalMove(index, pieceColor);
    }

    private void GenerateDiagonalMove(byte index, PieceColor pieceColor)
    {
        int[] directions = { -9, -7, 7, 9 };
        foreach (int direction in directions)
        {
            byte offset = 1;
            while (true)
            {
                byte newIndex = (byte)(index + direction * offset);

                // ѕроверка, что newIndex находитс€ в пределах доски (0..63)
                if (newIndex < 0 || newIndex > 63)
                    break;

                // ѕроверка, что фигура не вышла за пределы доски по горизонтали
                if (Mathf.Abs((newIndex % 8) - (index % 8)) != offset)
                    break;

                // ѕроверка, можно ли сделать ход на newIndex
                if (!pieceService.MoveChecker.IsMoveValid(index, newIndex))
                    break;

                // ƒобавление хода в список
                gameManager.Moves[index].Add(newIndex);

                // ≈сли на newIndex стоит фигура противника, прерываем цикл
                if (!PiecePacker.IsEqualType(gameManager.Board[newIndex], PieceType.None))
                    break;

                if (OnSide(newIndex))
                    break;

                offset++;
            }
        }
    }
    private void GenerateOrthogonalMove(byte index, PieceColor pieceColor)
    {
        int[] directions = { -8, -1, 1, 8 };
        foreach (int direction in directions)
        {
            byte offset = 1;
            while (true)
            {
                byte newIndex = (byte)(index + direction * offset);

                // ѕроверка, что newIndex находитс€ в пределах доски (0..63)
                if (newIndex < 0 || newIndex > 63)
                    break;

                // ѕроверка, что фигура не вышла за пределы доски по горизонтали
                if ((newIndex / 8 != index / 8) && (newIndex % 8 != index % 8))
                    break;

                // ѕроверка, можно ли сделать ход на newIndex
                if (!pieceService.MoveChecker.IsMoveValid(index, newIndex))
                    break;

                // ƒобавление хода в список
                gameManager.Moves[index].Add(newIndex);

                // ≈сли на newIndex стоит фигура противника, прерываем цикл
                if (!PiecePacker.IsEqualType(gameManager.Board[newIndex], PieceType.None))
                    break;

                //if (OnSide(newIndex))
                //    break;

                offset++;
            }
        }
    }


    private void CorrectMoves()
    {

    }

    private void ApplyMove(byte index, byte endIndex)
    {
        if (pieceService.MoveChecker.IsMoveValid(index, endIndex))
            gameManager.Moves[index].Add(endIndex);
    }

    private bool OnSide(int index) => OnRight(index) || OnLeft(index) || OnTop(index) || OnBottom(index);
    private bool OnRight(int index) => index % 8 == 7;
    private bool OnLeft(int index) => index % 8 == 0;
    private bool OnTop(int index) => index <= 7;
    private bool OnBottom(int index) => index >= 56;
}