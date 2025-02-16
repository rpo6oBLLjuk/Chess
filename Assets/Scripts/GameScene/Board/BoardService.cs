using UnityEngine;
using Zenject;

public class BoardService : MonoService
{
    [Inject] GameController gameController;
    [Inject] private PieceService pieceService;

    [SerializeField] private BoardBuilder boardBuilder;

    public CellHandler[,] cells;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        boardBuilder.Init(this, pieceService); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    public void Setup()
    {
        boardBuilder.SetupBoard(gameController.DeskData);
    }
}
