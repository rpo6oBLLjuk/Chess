using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private GameController gameController;


    private void Awake()
    {
        gameController.Setup();
    }
}
