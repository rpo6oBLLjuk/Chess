using UnityEngine;
using Zenject;

public class GameBootstrap : MonoBehaviour
{
    [Inject] private GameController gameController;


    private void Awake()
    {
        gameController.Setup();
    }
}
