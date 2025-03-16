using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeskSaverUI : AnimatedPanel
{
    [Inject] private GameManager gameManager;
    [Inject] private NotificationService notificationService;
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
        deskSaver.SaveBoard(gameManager.Board, saveNameInput.text);
    }
}
