using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/GameData")]
public class GameData : ScriptableObject
{
    public event Action<byte[]> BoardChanged;

    [field: SerializeField] public AllowCapture AllowCaptures { get; private set; }
    [field: SerializeField] public AllowMovement AllowMovement { get; set; }
    [field: SerializeField] public bool IgnoreMoveColors { get; private set; }

    [field: SerializeField] public byte[] LoadableBoard { get; private set; }


    public void SetBoard(byte[] board)
    {
        Array.Copy(board, LoadableBoard, board.Length);

        BoardChanged?.Invoke(board.ToArray());
    }
    public void SetDefaultBoard() => LoadableBoard = GetDefaultBoard();

    public byte[] GetBoard() => LoadableBoard.ToArray();
    public byte[] GetDefaultBoard()
    {
        byte[] board = new byte[64];
        #region Rooks
        board[ArrayWrapper.ConvertCoordinateToIndex(0, 0)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);
        board[ArrayWrapper.ConvertCoordinateToIndex(7, 0)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.Black);

        board[ArrayWrapper.ConvertCoordinateToIndex(0, 7)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        board[ArrayWrapper.ConvertCoordinateToIndex(7, 7)] = PiecePacker.PackPiece(PieceType.Rook, PieceColor.White);
        #endregion

        #region Knights
        board[ArrayWrapper.ConvertCoordinateToIndex(1, 0)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);
        board[ArrayWrapper.ConvertCoordinateToIndex(6, 0)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.Black);

        board[ArrayWrapper.ConvertCoordinateToIndex(1, 7)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        board[ArrayWrapper.ConvertCoordinateToIndex(6, 7)] = PiecePacker.PackPiece(PieceType.Knight, PieceColor.White);
        #endregion

        #region Bishops
        board[ArrayWrapper.ConvertCoordinateToIndex(2, 0)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);
        board[ArrayWrapper.ConvertCoordinateToIndex(5, 0)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.Black);

        board[ArrayWrapper.ConvertCoordinateToIndex(2, 7)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        board[ArrayWrapper.ConvertCoordinateToIndex(5, 7)] = PiecePacker.PackPiece(PieceType.Bishop, PieceColor.White);
        #endregion

        #region Queens
        board[ArrayWrapper.ConvertCoordinateToIndex(3, 0)] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.Black);
        board[ArrayWrapper.ConvertCoordinateToIndex(3, 7)] = PiecePacker.PackPiece(PieceType.Queen, PieceColor.White);
        #endregion

        #region Kings
        board[ArrayWrapper.ConvertCoordinateToIndex(4, 0)] = PiecePacker.PackPiece(PieceType.King, PieceColor.Black);
        board[ArrayWrapper.ConvertCoordinateToIndex(4, 7)] = PiecePacker.PackPiece(PieceType.King, PieceColor.White);
        #endregion

        #region Pawns
        for (byte i = 0; i < 8; i++)
        {
            board[ArrayWrapper.ConvertCoordinateToIndex(i, 1)] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.Black);
            board[ArrayWrapper.ConvertCoordinateToIndex(i, 6)] = PiecePacker.PackPiece(PieceType.Pawn, PieceColor.White);
        }
        #endregion

        return board;
    }
}