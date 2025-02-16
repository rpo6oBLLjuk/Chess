using UnityEngine;
using Zenject;

public class BoardService : MonoService
{
    [Inject] private PieceService pieceService;

    [SerializeField] private BoardBuilder boardBuilder;

    public CellHandler[,] cells;
    public Vector2Int BoardSize => boardBuilder.BoardSize;


    public void Setup(DeskData deskData)
    {
        boardBuilder.Init(this, pieceService);
        boardBuilder.SetupBoard(deskData);
    }
}
