using UnityEngine;
using UnityEngine.UI;

public class BoardService : MonoService
{
    //[Inject] GameController gameManager;

    [Header("References")]
    [SerializeField] GridLayoutGroup boardGridLayout;
    [SerializeField] GameObject cellPrefab;

    [field: Header("Data"), SerializeField]
    public CellsSkinData CellsSkinData { get; private set; }

    BoardBuilder boardBuilder;


    public override void Initialize()
    {
        base.Initialize();

        boardBuilder = container.Instantiate<BoardBuilder>();
        boardBuilder.Init(CellsSkinData, boardGridLayout, cellPrefab);
    }

    public void Setup() => boardBuilder.SetupBoard();
}
