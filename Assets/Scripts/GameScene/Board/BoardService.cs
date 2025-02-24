using UnityEngine;
using Zenject;

public class BoardService : MonoService
{
    [Inject] GameController gameController;

    [SerializeField] private BoardBuilder boardBuilder;
    public CellsSkinData cellsSkinData;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        boardBuilder.Init(gameController, cellsSkinData); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    public void Setup()
    {
        boardBuilder.SetupBoard();
    }
}
