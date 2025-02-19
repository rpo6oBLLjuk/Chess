using UnityEngine;
using Zenject;

public class BoardService : MonoService
{
    [Inject] GameController gameController;
    [Inject] private PieceService pieceService;

    [SerializeField] private BoardBuilder boardBuilder;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        boardBuilder.Init(this, pieceService, gameController); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    public void Setup()
    {
        boardBuilder.SetupBoard(gameController.PiecesData);
    }
}
