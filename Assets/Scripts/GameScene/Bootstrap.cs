using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private DeskSaverService deskSaver;
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private BoardManager boardManager;

    [Header("Custom Data")]
    [SerializeField] private bool forceLoadCustomData = false;
    [SerializeField] private DeskData deskData; //remove


    private void Awake()
    {
        DeskData data = deskSaver.LoadBoard("main", out string status);
        if (data != null)
        {
            boardManager.Setup(data);
            pieceManager.Setup(data);
        }
        else if (forceLoadCustomData)
        {
            boardManager.Setup(deskData);
            pieceManager.Setup(deskData);
        }
        else
        {
            Debug.Log(status);
        }
    }
}
