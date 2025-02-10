using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeskSaverUI : AnimatedPanel
{
    [SerializeField] private PopupService popupService;

    [Inject] private DeskSaverService deskSaver;

    [SerializeField] private TMP_InputField saveNameInput;
    [SerializeField] private Button saveButton;


    protected override void Start()
    {
        base.Start();
        saveButton.onClick.AddListener(Save);
    }

    private void Save()
    {
        PopupType saveStatus = deskSaver.SaveBoard(new DeskData(), saveNameInput.text, out string status);
        popupService.Show(status, "Saver", popupType: saveStatus);
    }
}
