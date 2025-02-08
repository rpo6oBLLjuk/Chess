using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private BoardCellBuilder boardBuilder;


    public void Setup(DeskData deskData)
    {
        boardBuilder.SetupBoard(deskData);
    }
}
