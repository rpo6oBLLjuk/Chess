using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private PieceBuilder PiecesBuilder;


    public void Setup(DeskData deskData)
    {
        PiecesBuilder.SetupPieces(deskData);
    }

    public void SpawnPiece(PieceType pieceType, PieceColor color)
    {

    }
}
