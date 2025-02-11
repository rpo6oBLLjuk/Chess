using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] PopupService popupService;

    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private BoardManager boardManager;

    [SerializeField] private Button defaultButton;
    [SerializeField] private DeskSaverUI saveUI;

    private Button saveButton;
    private Button colorButton;

    private PieceColor currectPieceColor = PieceColor.White;

    private void Awake()
    {
        defaultButton.gameObject.SetActive(false);

        foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
        {
            if (type != PieceType.None)
            {
                GameObject buttonInstance = InstantiateButton(type.ToString());
                buttonInstance.GetComponent<Button>().onClick.AddListener(() => SpawnerButtonCallback(type));
            }
        }

        saveButton = InstantiateButton("Save").GetComponent<Button>();
        saveButton.onClick.AddListener(() => saveUI.Show());

        colorButton = InstantiateButton("White").GetComponent<Button>();
        colorButton.onClick.AddListener(() =>
        {
            currectPieceColor = (currectPieceColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            colorButton.GetComponentInChildren<TextMeshProUGUI>().text = currectPieceColor.ToString();
        });
    }

    private GameObject InstantiateButton(string name)
    {
        GameObject buttonInstance = Instantiate(defaultButton.gameObject, defaultButton.transform.parent);
        buttonInstance.name = name;
        buttonInstance.SetActive(true);

        buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = name;

        return buttonInstance;
    }

    private void SpawnerButtonCallback(PieceType type)
    {
        var foundIndex = (from x in Enumerable.Range(0, pieceManager.DeskData.PieceData.GetLength(0))
                          from y in Enumerable.Range(0, pieceManager.DeskData.PieceData.GetLength(1))
                          where pieceManager.DeskData.PieceData[x, y] == null
                          select (x, y)).Cast<(int, int)?>().FirstOrDefault();

        if (!foundIndex.HasValue)
        {
            popupService.Show("Board full", "Spawner", PopupType.Error);
        }
        else
        {
            pieceManager.SpawnPiece(type, currectPieceColor, boardManager.cells[foundIndex.Value.Item1, foundIndex.Value.Item2]);
        }
    }
}
