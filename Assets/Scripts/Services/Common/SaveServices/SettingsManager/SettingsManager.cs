using CustomInspector;
using UnityEngine;
using Zenject;

public class SettingsManager : MonoService
{
    [field: Foldout] public SettingsData Settings { get; private set; }

    [Inject] NotificationService notificationService;

    [SerializeField] string saveDirectory = "Saves/Settings/";
    [SerializeField] string fileName = "settings";

    [SerializeField] SettingsData defaultData;


    public void Init()
    {

    }
}
