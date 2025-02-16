using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PieceSpawnerUI : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameController gameController;

    [Inject] PieceService pieceService;
    [Inject] BoardService boardService;

    [SerializeField] private Button defaultButton;
    [SerializeField] private Transform pieceButtonsParent;
    [SerializeField] private Transform systemButtonsParent;
    [SerializeField] private DeskSaverUI saveUI;

    private Button saveButton;
    private Button colorButton;

    private PieceColor currectPieceColor = PieceColor.White;

    private void Awake()
    {
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
        var foundIndex = (from x in Enumerable.Range(0, gameController.DeskData.BoardData.GetLength(0))
                          from y in Enumerable.Range(0, gameController.DeskData.BoardData.GetLength(1))
                          where gameController.DeskData.BoardData[x, y] == null
                          select (x, y)).Cast<(int, int)?>().FirstOrDefault();

        if (!foundIndex.HasValue)
        {
            notificationService.ShowPopup("Board full", "Spawner", PopupType.Error);
        }
        else
        {
            pieceService.SpawnPiece(type, currectPieceColor, boardService.cells[foundIndex.Value.Item1, foundIndex.Value.Item2]);
        }
    }
}
