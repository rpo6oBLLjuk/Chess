//using System.IO;
//using UnityEngine;
//using Zenject;

//public class BaseSaver<T> : MonoService where T : class
//{
//    [Inject] NotificationService notificationService;

//    [SerializeField] protected string _saveDirectory = "Saves/";
//    [SerializeField] protected string _fileName = "file.json";


//    public void Save(T obj)
//    {

//    }

//    public T Load(string fileName = default)
//    {
//        return T;

//    }

//    public bool DeleteSave(string fileName = default)
//    {
//        string fileName = GetFileName(fileName);

//        if (string.IsNullOrWhiteSpace(boardName))
//        {
//            notificationService.ShowPopup("Имя сохранения не может быть пустым!", "Saver", PopupType.Error);
//            return false;
//        }

//        string fileName = boardName + ".json";
//        string fullPath = Path.Combine(Application.persistentDataPath, _saveDirectory, fileName);

//        if (!File.Exists(fullPath))
//        {
//            notificationService.ShowPopup($"Файл сохранения {boardName} не найден!", "Saver", PopupType.Error);
//            return false;
//        }

//        File.Delete(fullPath);
//        notificationService.ShowPopup($"Файл сохранения {boardName} успешно удалён.", "Saver", PopupType.Info);

//        return true;
//    }

//    private void OverwriteSaveFile(bool overwrite, string fullPath, string json)
//    {
//        if (overwrite)
//        {
//            File.WriteAllText(fullPath, json);
//            notificationService.ShowPopup("File is overwritten", "Saver", PopupType.Info);
//        }
//        else
//        {
//            notificationService.ShowPopup("File is not overwritten", "Saver", PopupType.Info);
//        }
//    }

//    private bool GetFileName(string fileName, out string newFileName)
//    {
//        if (string.IsNullOrWhiteSpace(fileName))
//        {
//            notificationService.ShowPopup("Имя сохранения не может быть пустым!", "Saver", PopupType.Error);
//            if()
//            return _fileName;
//        }
//        return false;
//    }
//}
