using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private DeskSaverService deskSaver;
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private BoardManager boardManager;

    [SerializeField] private DeskData deskData; //remove


    private void Awake()
    {
        DeskData data = deskSaver.LoadBoard("main", out string status);
        if (data != null)
        {
            boardManager.Setup(data);
            pieceManager.Setup(data);
        }
        else
        {
            Debug.Log(status);
        }

        
    }
}
