using CustomInspector;
using System.IO;
using UnityEngine;
using Zenject;

public abstract class BaseSaver<T> : MonoService where T : struct
{
    [Inject] NotificationService notificationService;

    [SerializeField] protected string _saverName = "Saver";
    [SerializeField] protected string _baseDirectory = "Saves/";

    [SerializeField] bool showNotification = false;
    [SerializeField, ShowIf(nameof(showNotification))] protected string overwriteFileNotification = "Перезаписать файл?";
    [SerializeField, ShowIf(nameof(showNotification))] protected string fileOverwritedNotification = "Файл перезаписан";
    [SerializeField, ShowIf(nameof(showNotification))] protected string fileNotOverwritedNotification = "Файл не перезаписан";

    [SerializeField, ShowIf(nameof(showNotification))] protected string fileSavedNotification = "Файл сохранён";

    [SerializeField, ShowIf(nameof(showNotification))] protected string saveNotFoundNotification = "Файл сохранения {0} не найден";
    [SerializeField, ShowIf(nameof(showNotification))] protected string saveLoadedNotification = "Файл сохранения {0} успешно загружен";
    [SerializeField, ShowIf(nameof(showNotification))] protected string saveDeletedNotification = "Файл сохранения {0} успешно удалён";

    [SerializeField, ShowIf(nameof(showNotification))] protected string emptySaveName = "Имя сохранения не может быть пустым";



    public virtual bool Save(T obj, string saveName, bool forceOverwrite = false)
    {
        if (!FileNameIsCorrect(saveName))
            return false;

        GetFullPath(saveName, out string fullPath);
        ConvertDataToJson(obj, out string json);

        if (File.Exists(fullPath))
        {
            if (!forceOverwrite)
            {
                notificationService.ShowDialog((overwrite) => OverwriteFile(overwrite, fullPath, json), overwriteFileNotification, _saverName, DialogType.OkCancel);
                return false;
            }
            else
            {
                OverwriteFile(true, fullPath, json);
                return true;
            }
        }
        else
        {
            File.WriteAllText(fullPath, json);
            ShowNotification(fileSavedNotification, PopupType.Info);

            return true;
        }
    }
    public virtual T Load(string saveName = default)
    {
        GetFullPath(saveName, out string fullPath);

        if (!File.Exists(fullPath))
        {
            ShowNotification(string.Format(saveNotFoundNotification, saveName), PopupType.Error);
            return default;
        }
        else
        {
            string json = File.ReadAllText(fullPath);
            T data = ConvertJsonToData(json);

            ShowNotification(string.Format(saveLoadedNotification, saveName), PopupType.Info);

            return data;
        }
    }
    public virtual bool DeleteSave(string saveName)
    {
        if (!FileNameIsCorrect(saveName))
            return false;

        GetFullPath(saveName, out string fullPath);

        if (!File.Exists(fullPath))
        {
            ShowNotification(string.Format(saveNotFoundNotification, saveName), PopupType.Error);
            return false;
        }
        else
        {
            File.Delete(fullPath);
            ShowNotification(string.Format(saveDeletedNotification, saveName), PopupType.Info);

            return true;
        }
    }

    protected void Awake()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _baseDirectory);
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
    }

    protected void ConvertDataToJson(T data, out string json) => json = JsonUtility.ToJson(data, false);
    protected T ConvertJsonToData(string json) => JsonUtility.FromJson<T>(json);

    private void OverwriteFile(bool overwrite, string fullPath, string json)
    {
        if (overwrite)
        {
            File.WriteAllText(fullPath, json);
            ShowNotification(fileOverwritedNotification, PopupType.Info);
        }
        else
        {
            ShowNotification(fileNotOverwritedNotification, PopupType.Info);
        }
    }

    private bool FileNameIsCorrect(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            ShowNotification(emptySaveName, PopupType.Error);
            return false;
        }
        return true;
    }
    private string GetFullPath(string saveName, out string fullPath) => fullPath = Path.Combine(Application.persistentDataPath, _baseDirectory, $"{saveName}.json");

    private void ShowNotification(string notification, PopupType popupType)
    {
        if (showNotification)
            notificationService.ShowPopup(notification, _saverName, popupType);
    }
}
