using CustomInspector;
using UnityEngine;
using Zenject;

public class SettingsManager : MonoService
{
    [field: SerializeField, Foldout] public SettingsData Settings { get; private set; }

    [Inject] NotificationService notificationService;

    [SerializeField] string saveDirectory = "Saves/Settings/";
    [SerializeField] string fileName = "settings";

    [Button(nameof(SetGraphicsSettings))]
    [SerializeField] SettingsData defaultData;


    private void Awake()
    {
        SetGraphicsSettings();
    }

    public void UpdateVolumeData(float masterVolume, float musicVolume, float soundVolume)
    {

    }
    public void UpdateGraphicsData(bool useVSync, int targetFrameRate)
    {

    }
    public void UpdateLanguageData(string language)
    {

    }

    
    private void SetGraphicsSettings()
    {
        QualitySettings.vSyncCount = Settings.VSync ? 1 : 0;
        Application.targetFrameRate = Settings.TargetFrameRate;
        Screen.fullScreen = Settings.FullScreen;
    }
}
