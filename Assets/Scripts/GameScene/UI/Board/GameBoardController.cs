using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private BoardCellBuilder boardBuilder;

    void Start()
    {
        boardBuilder.SetupBoard();
    }
}
