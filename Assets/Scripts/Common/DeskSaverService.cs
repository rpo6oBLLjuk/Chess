using System.IO;
using UnityEngine;
using Zenject;

public class DeskSaverService : MonoService
{
    [Inject] NotificationService notificationService;

    [SerializeField] private string saveDirectory = "Saves/Desk/";

    public override void OnInstantiated()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory);
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
    }

    public bool? SaveBoard(BoardPiecesData boardData, string saveName)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            notificationService.ShowPopup("Имя сохранения не может быть пустым!", "Saver", PopupType.Error);

            return false;
        }

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        string json = JsonUtility.ToJson(boardData, true);

        Debug.Log(json);

        if (File.Exists(fullPath))
        {
            notificationService.ShowDialog((overwrite) => OverwriteSaveFile(overwrite, fullPath, json), "Перезаписать файл?", "Saver", DialogType.OkCancel);
            return null;
        }
        else
        {
            File.WriteAllText(fullPath, json);
            notificationService.ShowPopup($"Доска сохранена в {fullPath}", "Saver", PopupType.Info);
            return true;
        }
    }

    public BoardPiecesData LoadBoard(string saveName)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            notificationService.ShowPopup("Имя сохранения не может быть пустым!", "Saver", PopupType.Error);
            return null;
        }

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        if (!File.Exists(fullPath))
        {
            notificationService.ShowPopup($"Файл сохранения {saveName} не найден!", "Saver", PopupType.Error);
            return null;
        }

        string json = File.ReadAllText(fullPath);
        BoardPiecesData boardData = JsonUtility.FromJson<BoardPiecesData>(json);

        return boardData;
    }

    public void OverwriteSaveFile(bool overwrite, string fullPath, string json)
    {
        if (overwrite)
        {
            File.WriteAllText(fullPath, json);
            notificationService.ShowPopup("File is overwritten", "Saver", PopupType.Info);
        }
        else
        {
            notificationService.ShowPopup("File is not overwritten", "Saver", PopupType.Info);
        }
    }

    public string[] GetAllSaves()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory);
        if (!Directory.Exists(fullPath))
        {
            return new string[0];
        }

        string[] files = Directory.GetFiles(fullPath, "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }
}
