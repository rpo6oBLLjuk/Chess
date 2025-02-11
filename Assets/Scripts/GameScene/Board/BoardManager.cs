using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private PieceManager pieceManager;

    [SerializeField] private BoardBuilder boardBuilder;

    public CellHandler[,] cells;


    public void Setup(DeskData deskData)
    {
        boardBuilder.Init(this, pieceManager);
        boardBuilder.SetupBoard(deskData);
    }
}
