using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private DeskSaverService deskSaver;
    [Inject] private PopupService popupService;
    [Inject] private PieceService pieceService;
    [Inject] private BoardService boardService;

    [Header("Custom Data")]
    [SerializeField] private bool forceLoadCustomData = false;
    [SerializeField] private DeskData deskData; //remove


    private void Awake()
    {
        DeskData data = deskSaver.LoadBoard("main", out string status);

        if (forceLoadCustomData)
        {
            popupService.Show("Custom Data loaded", "Bootstrap", PopupType.Info);

            boardService.Setup(deskData);
            pieceService.Setup(deskData);
        }
        else if (data != null)
        {
            popupService.Show(status, "Bootstrap", PopupType.Info);

            boardService.Setup(data);
            pieceService.Setup(data);
        }
        else
        {
            popupService.Show(status, "Bootstrap", PopupType.Error);
        }
    }
}
