using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private FigureManager figuresController;
    [SerializeField] private BoardController boardController;

    [SerializeField] private DeskData deskData; //remove


    private void Awake()
    {
        boardController.Setup(deskData);
        figuresController.Setup(deskData);
    }
}
