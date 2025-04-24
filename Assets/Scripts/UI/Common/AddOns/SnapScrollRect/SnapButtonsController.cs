using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapButtonsController : MonoBehaviour
{
    [SerializeField] SnapScrollRect snapScrollRect;
    [SerializeField] List<Button> menuButtons;


    private void Awake()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            int index = i;
            Button button = menuButtons[index];

            button.onClick.AddListener(() =>
            {
                snapScrollRect.ScrollTo(index, index);
                Debug.Log($"Index: {index}");
            });

        }
    }
}
