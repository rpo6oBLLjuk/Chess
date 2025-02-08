using UnityEngine;

public class FigureManager : MonoBehaviour
{
    [SerializeField] private FiguresBuilder figuresBuilder;


    public void Setup(DeskData deskData)
    {
        figuresBuilder.SetupFigures(deskData);
    }
}
