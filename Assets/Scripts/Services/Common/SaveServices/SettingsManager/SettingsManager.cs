using CustomInspector;
using UnityEngine;
using Zenject;

public class SettingsManager : MonoService
{
    [field: SerializeField, Foldout, Inject] public SettingsData Settings { get; private set; }

    [Inject] NotificationService notificationService;

    [SerializeField] string saveDirectory = "Saves/Settings/";
    [SerializeField] string fileName = "settings";

    [Button(nameof(SaveDefaultSettings))]
    [SerializeField] SettingsData defaultData;


    public void UpdateVolumeData(float masterVolume, float musicVolume, float soundVolume) => Settings.UpdateAudioSettings(masterVolume, musicVolume, soundVolume);
    public void UpdateGraphicsData(bool useVSync, int targetFrameRate, bool fullScreen) => Settings.UpdateGraphicsSettings(useVSync, targetFrameRate, fullScreen);
    public void UpdateLanguageData(string language) => Settings.UpdateLanguageSettings(language);

    private void SaveDefaultSettings() => Settings.UpdateFromOtherConfig(defaultData);
}
