using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    [SerializeField] private DeskSaverUI saveUI;

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

        GameObject saveButton = InstantiateButton("Save");
        saveButton.GetComponent<Button>().onClick.AddListener(() => saveUI.Show());

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
        Debug.Log($"Button {type} on click");
    }
}
