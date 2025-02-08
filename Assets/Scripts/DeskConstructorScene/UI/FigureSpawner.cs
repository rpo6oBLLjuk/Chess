using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FigureSpawner : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    [SerializeField] private SaveUI saveUI;

    private void Awake()
    {
        defaultButton.gameObject.SetActive(false);

        foreach (FigureType type in Enum.GetValues(typeof(FigureType)))
        {
            if (type != FigureType.None)
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

    private void SpawnerButtonCallback(FigureType type)
    {
        Debug.Log($"Button {type} on click");
    }
}
