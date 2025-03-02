using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConstructorUI : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameController gameController;

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

        foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
        {
            if (type != PieceType.None && type != PieceType.Other)
                FastInstantiateButton(type.ToString(), pieceButtonsParent, () => SpawnerButtonCallback(type));
        }

        colorButton = FastInstantiateButton("White", systemButtonsParent, () =>
        {
            currectPieceColor = (currectPieceColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            TextMeshProUGUI tmpro = colorButton.GetComponentInChildren<TextMeshProUGUI>();
            tmpro.text = currectPieceColor.ToString();
            tmpro.color = (currectPieceColor == PieceColor.White) ? Color.white : Color.black;
            colorButton.GetComponent<Image>().sprite = (currectPieceColor == PieceColor.White) ? whiteColorSprite : blackColorSprite;
        });
        destroyButton = FastInstantiateButton("Destroy (inactive)", systemButtonsParent, () => DestroyButtonCallback());

        saveButton = FastInstantiateButton("Save", systemButtonsParent, () => saverUI.Show());
        loadButton = FastInstantiateButton("Load", systemButtonsParent, () => loaderUI.Show());

        clearBoardButton = FastInstantiateButton("Clear board", systemButtonsParent, () => gameController.ClearBoard());
    }

    private Button FastInstantiateButton(string name, Transform parent, Action callback)
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

    private void SpawnerButtonCallback(PieceType type)
    {
        var foundIndex = gameController.PiecesData.Data.FindIndex(piece => piece.Type == PieceType.None);

        if (foundIndex == -1)
        {
            notificationService.ShowPopup("Board full", "Spawner", PopupType.Error);
        }
        else
        {
            gameController.SpawnPiece(type, currectPieceColor, gameController.CellsData.Data[foundIndex]);
        }
    }

    private void DestroyButtonCallback()
    {
        destroyerIsActive = !destroyerIsActive;

        if (destroyerIsActive)
        {
            gameController.CellClicked += DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (active)";
        }
        else
        {
            gameController.CellClicked -= DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (inactive)";
        }
    }
    private void DestroyPiece(CellHandler cellHandler)
    {
        gameController.DestroyPiece(cellHandler);
    }
}
