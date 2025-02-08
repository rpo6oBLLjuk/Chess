using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : AnimatedPanel
{
    [SerializeField] private PopupService popupService;

    [SerializeField] private SaveManager saveManager;

    [SerializeField] private TMP_InputField saveNameInput;
    [SerializeField] private Button saveButton;


    private void Start()
    {
        saveButton.onClick.AddListener(Save);
    }

    private void Save()
    {
        bool isCorrectSave = saveManager.SaveBoard(new DeskData(), saveNameInput.text, out string status);
        popupService.Show(status, popupType: isCorrectSave ? PopupType.Info : PopupType.Error);
    }
}
