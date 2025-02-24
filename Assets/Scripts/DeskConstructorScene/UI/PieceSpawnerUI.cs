using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceSpawnerUI : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameController gameController;

    [SerializeField] private Button defaultButton;
    [SerializeField] private Transform pieceButtonsParent;
    [SerializeField] private Transform systemButtonsParent;
    [SerializeField] private DeskSaverUI saveUI;

    private Button saveButton;
    private Button colorButton;
    private Button destroyButton;

    private bool destroyerIsActive;

    private PieceColor currectPieceColor = PieceColor.White;


    private void Awake()
    {
        destroyerIsActive = false;

        defaultButton.gameObject.SetActive(false);

        foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
        {
            if (type != PieceType.None && type != PieceType.Other)
            {
                GameObject buttonInstance = InstantiateButton(type.ToString(), pieceButtonsParent);
                buttonInstance.GetComponent<Button>().onClick.AddListener(() => SpawnerButtonCallback(type));
            }
        }

        saveButton = InstantiateButton("Save", systemButtonsParent).GetComponent<Button>();
        saveButton.onClick.AddListener(() => saveUI.Show());

        colorButton = InstantiateButton("White", systemButtonsParent).GetComponent<Button>();
        colorButton.onClick.AddListener(() =>
        {
            currectPieceColor = (currectPieceColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            colorButton.GetComponentInChildren<TextMeshProUGUI>().text = currectPieceColor.ToString();
        });

        destroyButton = InstantiateButton("Destroy (inactive)", systemButtonsParent).GetComponent<Button>();
        destroyButton.onClick.AddListener(() => DestroyButtonCallback());
    }

    private GameObject InstantiateButton(string name, Transform parent)
    {
        GameObject buttonInstance = Instantiate(defaultButton.gameObject, parent);
        buttonInstance.name = name;
        buttonInstance.SetActive(true);

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
            gameController.PiecePointerClicked += DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (active)";
        }
        else
        {
            gameController.PiecePointerClicked -= DestroyPiece;
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy (inactive)";
        }
    }
    private void DestroyPiece(PieceHandler pieceHandler)
    {
        gameController.DestroyPiece(pieceHandler);
    }
}
