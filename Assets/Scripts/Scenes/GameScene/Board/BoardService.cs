using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BoardService : MonoService
{
    [Inject] GameController gameController;

    [Header("References")]
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private BoardBuilder boardBuilder;
    public CellsSkinData cellsSkinData;


    public override void OnInstantiated()
    {
        base.OnInstantiated();

        boardBuilder = container.Instantiate<BoardBuilder>();
        boardBuilder.Init(cellsSkinData, boardGridLayout, cellPrefab);
    }

    public void Setup()
    {
        boardBuilder.SetupBoard();
    }
}
