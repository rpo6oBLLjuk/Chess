using TMPro;
using UnityEngine;
using Zenject;

public class TurnColorWidget : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] string defaultString = "";

    private void OnEnable() => gameManager.GameTurnController.OnTurnChanged += SetTurnColor;
    private void OnDisable() => gameManager.GameTurnController.OnTurnChanged -= SetTurnColor;

    private void Start() => SetTurnColor(gameManager.GameTurnController.TurnColor);

    private void SetTurnColor(PieceColor color) => turnText.text = $"{defaultString}{color}";
}
