using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapButtonsController : MonoBehaviour
{
    [SerializeField] SnapScrollRect snapScrollRect;
    [SerializeField] List<Button> menuButtons;

    [SerializeField] bool logging = false;

    private void Awake()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            int index = i;
            Button button = menuButtons[index];

            button.onClick.AddListener(() =>
            {
                snapScrollRect.ScrollTo(index, index);

                if (logging)
                    Debug.Log($"Index: {index}");
            });

        }
    }
}
