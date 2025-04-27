using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MovesGenerator
{
    [Inject] GameManager gameManager;
    PieceService pieceService;


    public void Init(PieceService pieceService) => this.pieceService = pieceService;

    public void GenerateAllPossibleMoves(PieceColor pieceColor) => GenerateAllPossibleMoves(gameManager.Board, out gameManager.Moves, pieceColor);
    public void GenerateAllPossibleMoves(byte[] board, out List<byte>[] moves, PieceColor pieceColor, bool recursion = true)
    {
        moves = new List<byte>[64];
        if (!pieceService.MoveChecker.IsMovementAllowed())
            return;

        //Generate incorrect moves
        for (byte index = 0; index < 64; index++)
        {
            moves[index] = new();
            if (PiecePacker.IsDefaultPiece(board[index]) && (PiecePacker.IsEqualColor(board[index], pieceColor) || gameManager.GameData.IgnoreMoveColors))
            {
                GeneratePossibleMovesForPiece(index, board, moves);
            }
        }

        //Correcting moves
        if (recursion)
            CorrectionMoves(board, moves, pieceColor);
    }

    private void GeneratePossibleMovesForPiece(byte index, byte[] board, List<byte>[] moves)
    {
        if (gameManager.GameData.AllowMovement == AllowMovement.All)
        {
            moves[index] = Enumerable.Range(0, 64)
                .Where(x => pieceService.MoveChecker.IsCaptureAllowed(board[index], board[x]))
                .Select(x => (byte)x)
                .ToList();
            return;
        }

        PiecePacker.GetColor(board[index], out PieceColor pieceColor);

        switch (PiecePacker.GetType(board[index]))
        {
            case PieceType.Pawn:
                GeneratePawnMoves(index, pieceColor, board, moves);
                break;
            case PieceType.Knight:
                GenerateKnightMoves(index, board, moves);
                break;
            case PieceType.Bishop:
                GenerateBishopMoves(index, board, moves);
                break;
            case PieceType.Rook:
                GenerateRookMoves(index, board, moves);
                break;
            case PieceType.Queen:
                GenerateQueenMoves(index, board, moves);
                break;
            case PieceType.King:
                GenerateKingMoves(index, board, moves);
                break;
        }
    }

    private void GeneratePawnMoves(byte index, PieceColor pieceColor, byte[] board, List<byte>[] moves)
    {
        if (pieceColor == PieceColor.White)
        {
            if (OnTop(index))
            {
                this.LogWarning("Pawn_White on the top row");
                return;
            }

            if (PiecePacker.IsEqualType(board[index - 8], PieceType.None))
            {
                moves[index].Add((byte)(index - 8));
                if (index >= 48 && index <= 55)
                {
                    moves[index].Add((byte)(index - 16));
                }
            }

            if (!OnLeft(index))
            {
                if (PiecePacker.IsDefaultPiece(board[index - 9]))
                    ApplyMove(index, (byte)(index - 9), board, moves);
            }

            if (!OnRight(index))
            {
                if (PiecePacker.IsDefaultPiece(board[index - 7]))
                    ApplyMove(index, (byte)(index - 7), board, moves);
            }
        }
        else //if (pieceColor == PieceColor.Black)
        {
            if (OnBottom(index))
            {
                this.LogWarning("Pawn_Black on the bottom row");
                return;
            }

            if (PiecePacker.IsEqualType(board[index + 8], PieceType.None))
            {
                moves[index].Add((byte)(index + 8));

                if (index >= 8 && index <= 15 && PiecePacker.IsEqualType(board[index + 16], PieceType.None))
                {
                    moves[index].Add((byte)(index + 16));
                }
            }

            if (!OnLeft(index))
            {
                if (PiecePacker.IsDefaultPiece(board[index + 7]))
                    ApplyMove(index, (byte)(index + 7), board, moves);
            }

            if (!OnRight(index))
            {
                if (PiecePacker.IsDefaultPiece(board[index + 9]))
                    ApplyMove(index, (byte)(index + 9), board, moves);
            }
        }
    }

    private void GenerateKnightMoves(byte index, byte[] board, List<byte>[] moves)
    {
        if (!OnLeft(index))
        {
            if (index >= 16)
                ApplyMove(index, (byte)(index - 8 - 8 - 1), board, moves);

            if (index <= 47)
                ApplyMove(index, (byte)(index + 8 + 8 - 1), board, moves);
        }
        if (!OnRight(index))
        {
            if (index >= 16)
                ApplyMove(index, (byte)(index - 8 - 8 + 1), board, moves);

            if (index <= 47)
                ApplyMove(index, (byte)(index + 8 + 8 + 1), board, moves);
        }

        if (!OnTop(index))
        {
            if (index % 8 > 1)
                ApplyMove(index, (byte)(index - 1 - 1 - 8), board, moves);

            if (index % 8 < 6)
                ApplyMove(index, (byte)(index + 1 + 1 - 8), board, moves);
        }
        if (!OnBottom(index))
        {
            if (index % 8 > 1)
                ApplyMove(index, (byte)(index - 1 - 1 + 8), board, moves);

            if (index % 8 < 6)
                ApplyMove(index, (byte)(index + 1 + 1 + 8), board, moves);
        }
    }
    private void GenerateKingMoves(byte index, byte[] board, List<byte>[] moves)
    {
        bool onLeft = OnLeft(index);
        bool onRight = OnRight(index);
        bool onTop = OnTop(index);
        bool onBottom = OnBottom(index);

        if (!onTop)
        {
            if (!onLeft)
                ApplyMove(index, (byte)(index - 9), board, moves);

            if (!onRight)
                ApplyMove(index, (byte)(index - 7), board, moves);

            ApplyMove(index, (byte)(index - 8), board, moves);
        }
        if (!onBottom)
        {
            if (!onLeft)
                ApplyMove(index, (byte)(index + 7), board, moves);

            if (!onRight)
                ApplyMove(index, (byte)(index + 9), board, moves);

            ApplyMove(index, (byte)(index + 8), board, moves);
        }

        if (!onLeft)
            ApplyMove(index, (byte)(index - 1), board, moves);
        if (!onRight)
            ApplyMove(index, (byte)(index + 1), board, moves);
    }

    private void GenerateBishopMoves(byte index, byte[] board, List<byte>[] moves) => GenerateDiagonalMove(index, board, moves);
    private void GenerateRookMoves(byte index, byte[] board, List<byte>[] moves) => GenerateOrthogonalMove(index, board, moves);
    private void GenerateQueenMoves(byte index, byte[] board, List<byte>[] moves)
    {
        GenerateDiagonalMove(index, board, moves);
        GenerateOrthogonalMove(index, board, moves);
    }

    private void GenerateDiagonalMove(byte index, byte[] board, List<byte>[] moves)
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
                if (!pieceService.MoveChecker.IsMoveValid(board[index], board[newIndex]))
                    break;

                // ƒобавление хода в список
                moves[index].Add(newIndex);

                // ≈сли на newIndex стоит фигура противника, прерываем цикл
                if (!PiecePacker.IsEqualType(board[newIndex], PieceType.None))
                    break;

                if (OnSide(newIndex))
                    break;

                offset++;
            }
        }
    }
    private void GenerateOrthogonalMove(byte index, byte[] board, List<byte>[] moves)
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
                if (!pieceService.MoveChecker.IsMoveValid(board[index], board[newIndex]))
                    break;

                // ƒобавление хода в список
                moves[index].Add(newIndex);

                // ≈сли на newIndex стоит фигура противника, прерываем цикл
                if (!PiecePacker.IsEqualType(board[newIndex], PieceType.None))
                    break;

                //if (OnSide(newIndex))
                //    break;

                offset++;
            }
        }
    }

    private void ApplyMove(byte index, byte endIndex, byte[] board, List<byte>[] moves)
    {
        if (pieceService.MoveChecker.IsMoveValid(board[index], board[endIndex]))
            moves[index].Add(endIndex);
    }

    private bool OnSide(int index) => OnRight(index) || OnLeft(index) || OnTop(index) || OnBottom(index);
    private bool OnRight(int index) => index % 8 == 7;
    private bool OnLeft(int index) => index % 8 == 0;
    private bool OnTop(int index) => index <= 7;
    private bool OnBottom(int index) => index >= 56;

    private void CorrectionMoves(byte[] board, List<byte>[] moves, PieceColor currentPlayerColor)
    {
        if (gameManager.GameData.AllowMovement == AllowMovement.All)
            return;

        bool isChecked = false;

        PieceColor opponentColor = currentPlayerColor.Invert();

        byte[] fantomBoard = (byte[])board.Clone();
        GenerateAllPossibleMoves(fantomBoard, out var opponentResponses, opponentColor, false);
        if (IsKingUnderCheck(fantomBoard, opponentResponses, currentPlayerColor))
        {
            isChecked = true;
        }

        //ќтбраковка всех неверных мувов, привод€щих к самошаху
        for (int i = 0; i < moves.Length; i++)
        {
            var movesCopy = moves[i].ToList();

            foreach (byte move in movesCopy)
            {
                fantomBoard = (byte[])board.Clone();
                fantomBoard[move] = fantomBoard[i];
                fantomBoard[i] = 0;

                GenerateAllPossibleMoves(fantomBoard, out opponentResponses, opponentColor, false);

                if (IsKingUnderCheck(fantomBoard, opponentResponses, currentPlayerColor))
                {
                    moves[i].Remove(move);

                    //this.Log($"Move {i}->{move} leads to check King_{currentPlayerColor}");
                }
            }
        }

        if (isChecked)
            this.Log($"Check for {currentPlayerColor}");

        foreach (var move in moves)
        {
            if (move?.Count > 0)
                return;
        }

        if (isChecked)
        {
            this.Log($"Checkmate for {currentPlayerColor}");
            gameManager.GameEnd(opponentColor, false);
        }
        else
        {
            this.Log($"Pat for {currentPlayerColor}");
            gameManager.GameEnd(opponentColor, true);
        }
    }


    private bool IsKingUnderCheck(byte[] board, List<byte>[] moves, PieceColor kingColor)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == null || moves[i].Count == 0)
                continue;

            foreach (byte move in moves[i])
            {
                if (PiecePacker.IsEqualType(board[move], PieceType.King) && PiecePacker.IsEqualColor(board[move], kingColor))
                {
                    return true;
                }
            }
        }
        return false;
    }
}