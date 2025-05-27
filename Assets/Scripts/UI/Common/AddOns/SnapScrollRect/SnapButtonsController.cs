using CustomInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapButtonsController : MonoBehaviour
{
    [SerializeField] SnapScrollRect snapScrollRect;
    [SerializeField] List<Button> menuButtons;

    [SerializeField] Color activeColor;
    Color defaultColor;

    [SerializeField] bool autoSetFirstIndex;
    [SerializeField, ShowIf(nameof(autoSetFirstIndex))] int firstIndex = 0;
    [SerializeField] bool logging = false;


    private void OnEnable() => snapScrollRect.IndexChanged += IndexChanged;
    private void OnDisable() => snapScrollRect.IndexChanged -= IndexChanged;

    private void Awake()
    {
        defaultColor = menuButtons[0].GetComponent<Image>().color;

        for (int i = 0; i < menuButtons.Count; i++)
        {
            int index = i;

            Button button = menuButtons[index];

            button.onClick.AddListener(() =>
            {
                ActivateButton(button);

                snapScrollRect.ScrollTo(index, index);

                if (logging)
                    Debug.Log($"Index: {index}");
            });

        }

        if (autoSetFirstIndex)
            menuButtons[firstIndex].onClick.Invoke();
    }

    private void IndexChanged(Vector2Int index)
    {
        ActivateButton(menuButtons[index.x]);
    }

    private void ActivateButton(Button nextButton)
    {
        foreach (Button b in menuButtons)
            b.GetComponent<Image>().color = defaultColor;
        nextButton.GetComponent<Image>().color = activeColor;
    }
}
