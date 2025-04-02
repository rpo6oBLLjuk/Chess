using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameEndPanel : AnimatedPanel
{
    [Inject] GameManager gameManager;

    [Header("References")]
    [SerializeField] TextMeshProUGUI gameEndTypeText;
    [SerializeField] TextMeshProUGUI winnerColorText;

    [Header("Values")]
    [SerializeField] string checkmateText = "Win";
    [SerializeField] string patText = "Pat";

    [Space]
    [SerializeField] Button exitButton;


    private void OnEnable() => gameManager.GameEnded += GameEnd;
    private void OnDisable() => gameManager.GameEnded -= GameEnd;

    private void GameEnd(PieceColor pieceColor, bool pat)
    {
        Show();

        gameEndTypeText.text = pat ? patText : checkmateText;
        winnerColorText.text = pieceColor.ToString();

        exitButton.onClick.AddListener(CustomHide);
    }

    public void CustomHide()
    {
        SceneManager.LoadScene(0);
    }
}
