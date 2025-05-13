using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameEndPanel : AnimatedPanel
{
    [Inject] GameManager gameManager;
    [Inject] SceneLoader sceneLoader;

    [Header("References")]
    [SerializeField] TextMeshProUGUI gameEndTypeTMP;
    [SerializeField] TextMeshProUGUI winnerTMP;

    [Header("Values")]
    [SerializeField] string checkmateText = "Checkmate";
    [SerializeField] string patText = "Pat";

    [Space]
    [SerializeField] string winnerText = "Winner: ";

    [Space]
    [SerializeField] Button exitButton;


    private void OnEnable() => gameManager.GameEnded += GameEnd;
    private void OnDisable() => gameManager.GameEnded -= GameEnd;

    private void GameEnd(PieceColor pieceColor, bool pat)
    {
        this.Log("GameEnd panel showed");
        AnimShow();

        gameEndTypeTMP.text = pat ? patText : checkmateText;
        winnerTMP.text = $"{winnerText}{pieceColor}";

        exitButton.onClick.AddListener(AnimHide);
        exitButton.onClick.AddListener(CustomHide);
    }

    private void CustomHide()
    {
        sceneLoader.LoadMainScene();
    }
}
