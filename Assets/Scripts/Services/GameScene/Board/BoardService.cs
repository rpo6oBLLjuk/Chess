using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BoardService : MonoService
{
    [Header("References")]
    [SerializeField] GridLayoutGroup boardGridLayout;
    [SerializeField] GameObject cellPrefab;

    BoardBuilder boardBuilder;


    public override void Initialize()
    {
        base.Initialize();

        boardBuilder = container.Instantiate<BoardBuilder>();
        boardBuilder.Init(boardGridLayout, cellPrefab);
    }

    public void Setup() => boardBuilder.SetupBoard();
}
