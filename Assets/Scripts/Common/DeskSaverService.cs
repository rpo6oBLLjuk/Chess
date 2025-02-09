using System.IO;
using UnityEngine;

public class DeskSaverService : MonoBehaviour
{
    [SerializeField] private string saveDirectory = "Saves/Desk/";


    private void Awake()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory);
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
    }

    public bool SaveBoard(DeskData boardData, string saveName, out string status)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            status = "��� ���������� �� ����� ���� ������!"; //hardcode
            return false;
        }
        

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        if (File.Exists(fullPath))
        {
            status = "���� � ����� ������ ��� ����������!";
            return false;
        }

        string json = JsonUtility.ToJson(boardData, true);

        File.WriteAllText(fullPath, json);
        status = $"����� ��������� � {fullPath}";
        return true;
    }

    public DeskData LoadBoard(string saveName, out string status)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            status = "��� ���������� �� ����� ���� ������!";
            return null;
        }

        string fileName = saveName + ".json";
        string fullPath = Path.Combine(Application.persistentDataPath, saveDirectory, fileName);

        if (!File.Exists(fullPath))
        {
            status = $"���� ���������� {saveName} �� ������!";
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
