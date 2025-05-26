using CustomInspector;
using UnityEngine;
using Zenject;

public class SettingsManager : MonoService
{
    [field: SerializeField, Foldout, Inject] public SettingsData Settings { get; private set; }

    [Inject] NotificationService notificationService;

    [SerializeField] string saveDirectory = "Saves/Settings/";
    [SerializeField] string fileName = "settings";

    [Button(nameof(ApplyGraphicsSettings))]
    [Button(nameof(SaveDefaultSettings))]
    [SerializeField] SettingsData defaultData;


    public void Awake() => ApplyGraphicsSettings();

    public void UpdateVolumeData(float masterVolume, float musicVolume, float soundVolume) => Settings.UpdateAudioSettings(masterVolume, musicVolume, soundVolume);
    public void UpdateGraphicsData(bool useVSync, int targetFrameRate, bool fullScreen) => Settings.UpdateGraphicsSettings(useVSync, targetFrameRate, fullScreen);
    public void UpdateLanguageData(string language) => Settings.UpdateLanguageSettings(language);

    private void ApplyGraphicsSettings()
    {
        QualitySettings.vSyncCount = Settings.VSync ? 1 : 0;
        Application.targetFrameRate = Settings.TargetFrameRate;
        Screen.fullScreen = Settings.FullScreen;
    }

    private void SaveDefaultSettings() => Settings.UpdateFromOtherConfig(defaultData);
}
