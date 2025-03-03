using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeskLoaderUI : AnimatedPanel
{
    [Inject] private GameController gameController;
    [Inject] private NotificationService notificationService;
    [Inject] private DeskSaverService deskSaver;

    public GameObject defaultSave;
    
    private List<GameObject> pool = new();


    private void Awake()
    {
        defaultSave.SetActive(false);
    }

    public override void Show()
    {
        ResetPool();
        if (CreateSavesButtons())
            base.Show();
    }

    private void ResetPool()
    {
        pool.ForEach(saveObj => Destroy(saveObj));
        pool.Clear();
    }

    private bool CreateSavesButtons()
    {
        List<string> files = deskSaver.GetAllSaves();

        if (files.Count == 0)
        {
            notificationService.ShowPopup("There are no save files", "Loader", PopupType.Info);
            return false;
        }

        files.ForEach(file =>
        {
            GameObject saveObj = Instantiate(defaultSave, defaultSave.transform.parent);
            saveObj.SetActive(true);
            pool.Add(saveObj);

            saveObj.transform.FindDeepChild("TitleText").GetComponent<TextMeshProUGUI>().text = file;
            saveObj.transform.FindDeepChild("ApplyButton").GetComponentInChildren<Button>().onClick.AddListener(() => ApplyButtonClickCallback(file));
            saveObj.transform.FindDeepChild("DeleteButton").GetComponentInChildren<Button>().onClick.AddListener(() => DeleteButtonClickCallback(file, saveObj));
        });

        return true;
    }

    private void ApplyButtonClickCallback(string saveName)
    {
        gameController.SetCustomBoard(deskSaver.LoadBoard(saveName));
        Hide();
    }

    private void DeleteButtonClickCallback(string savename, GameObject saveObj)
    {
        notificationService.ShowDialog((confirmed) =>
        {
            if (confirmed)
            {
                DestroyImmediate(saveObj);
                deskSaver.DeleteBoard(savename);
            }
        }, "Are you sure you want to delete this desk?", "Delete File", DialogType.OkCancel);
    }
}
