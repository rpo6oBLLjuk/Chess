using UnityEngine;

public class FiguresController : MonoBehaviour
{
    [SerializeField] private FiguresBuilder figuresBuilder;
    [SerializeField] private DeskData deskData;

    private void Start()
    {
        figuresBuilder.SetupFigures(deskData);
    }
}
