using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private GameController gameController;


    private void Awake() => gameController.Setup();
}
