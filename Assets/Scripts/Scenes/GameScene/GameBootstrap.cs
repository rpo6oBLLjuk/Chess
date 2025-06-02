using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;


    private void Start() => gameManager.Setup();
}
