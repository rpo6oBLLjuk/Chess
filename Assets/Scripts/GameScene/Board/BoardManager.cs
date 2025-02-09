using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardBuilder boardBuilder;


    public void Setup(DeskData deskData)
    {
        boardBuilder.SetupBoard(deskData);
    }
}
