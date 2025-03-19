using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConstructorUI : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameManager gameManager;

    [SerializeField] private Button defaultButton;
    [SerializeField] private Transform pieceButtonsParent;
    [SerializeField] private Transform systemButtonsParent;

    [SerializeField] private DeskSaverUI saverUI;
    [SerializeField] private DeskLoaderUI loaderUI;

    private Button saveButton;
    private Button loadButton;
    private Button colorButton;
    private Button destroyButton;
    private Button clearBoardButton;

    [SerializeField] private Sprite whiteColorSprite;
    [SerializeField] private Sprite blackColorSprite;

    private bool destroyerIsActive;

    private PieceColor currectPieceColor = PieceColor.White;


    private void Awake()
    {
        destroyerIsActive = false;

        defaultButton.gameObject.SetActive(false);


        foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
        {
            if (pieceType != PieceType.None && pieceType != PieceType.Other)
                InstantiateButtonWithCallback(pieceType.ToString(), pieceButtonsParent, () => SpawnerButtonCallback(pieceType));
        }

        colorButton = InstantiateButtonWithCallback("White", systemButtonsParent, () =>
        {
            currectPieceColor = (currectPieceColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;

            TextMeshProUGUI tmpro = colorButton.GetComponentInChildren<TextMeshProUGUI>();
            tmpro.text = currectPieceColor.ToString();
            tmpro.color = (currectPieceColor == PieceColor.White) ? Color.white : Color.black;

            colorButton.GetComponent<Image>().sprite = (currectPieceColor == PieceColor.White) ? whiteColorSprite : blackColorSprite;
        });
        destroyButton = InstantiateButtonWithCallback("Destroy (inactive)", systemButtonsParent, () => DestroyButtonCallback());

        saveButton = InstantiateButtonWithCallback("Save", systemButtonsParent, () => saverUI.Show());
        loadButton = InstantiateButtonWithCallback("Load", systemButtonsParent, () => loaderUI.Show());

        clearBoardButton = InstantiateButtonWithCallback("Clear board", systemButtonsParent, () => gameManager.ClearBoard());
    }

    private Button InstantiateButtonWithCallback(string name, Transform parent, Action callback)
    {
        Button buttonInstance = InstantiateButton(name, parent).GetComponent<Button>();
        buttonInstance.onClick.AddListener(() => callback.Invoke());

        return buttonInstance;
    }

    private GameObject InstantiateButton(string name, Transform parent)
    {
        GameObject buttonInstance = Instantiate(defaultButton.gameObject, parent);
        buttonInstance.SetActive(true);

        buttonInstance.name = $"{name}Button";
        buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = name;

        return buttonInstance;
    }

    private void SpawnerButtonCallback(PieceType pieceType)
    {
        var foundIndex = Array.FindIndex(gameManager.Board, piece => PiecePacker.IsEqualType( piece, PieceType.None));

        if (foundIndex == -1)
            notificationService.ShowPopup("Board full", "Spawner", PopupType.Error);
        else
            gameManager.SpawnPiece(PiecePacker.PackPiece(pieceType, currectPieceColor), gameManager.Cells[foundIndex]);
    }

    private void DestroyButtonCallback()
    {
        destroyerIsActive = !destroyerIsActive;

        if (destroyerIsActive)
        {
            gameManager.CellClicked += DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (active)";
        }
        else
        {
            gameManager.CellClicked -= DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (inactive)";
        }
    }
    private void DestroyPiece(CellHandler cellHandler)
    {
        if (!PiecePacker.IsEqualType( gameManager.Board[cellHandler.Index], PieceType.None))
            gameManager.DestroyPiece(cellHandler);
    }
}
