using System.IO;
using UnityEngine;

public class DeskSaverService : MonoService
{
    [SerializeField] private string saveDirectory = "Saves/Desk/";

    public override void OnInstantiated()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory);
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
    }

    public PopupType SaveBoard(DeskData boardData, string saveName, out string status)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            status = "Имя сохранения не может быть пустым!"; //hardcode
            return PopupType.Error;
        }
        

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        if (File.Exists(fullPath))
        {
            status = "Файл с таким именем уже существует!";
            return PopupType.Warning;
        }

        string json = JsonUtility.ToJson(boardData, true);

        File.WriteAllText(fullPath, json);
        status = $"Доска сохранена в {fullPath}";
        return PopupType.Info;
    }

    public DeskData LoadBoard(string saveName, out string status)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            status = "Имя сохранения не может быть пустым!";
            return null;
        }

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        if (!File.Exists(fullPath))
        {
            status = $"Файл сохранения {saveName} не найден!";
            return null;
        }

        string json = File.ReadAllText(fullPath);
        DeskData boardData = JsonUtility.FromJson<DeskData>(json);

        status = "Board Loaded";

        return boardData;
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
