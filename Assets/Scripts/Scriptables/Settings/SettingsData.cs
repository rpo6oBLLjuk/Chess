using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/Settings")]
public class SettingsData : ScriptableObject
{
    [field: HorizontalLine("Audio")]
    [field: SerializeField, Range(0, 100)] public float MasterVolume { get; private set; }
    [field: SerializeField, Range(0, 100)] public float MusicVolume { get; private set; }
    [field: SerializeField, Range(0, 100)] public float SoundsVolume { get; private set; }

    [field: HorizontalLine("Graphics")]
    [field: SerializeField] public bool VSync { get; private set; } = true;
    [field: SerializeField, FixedValues(10, 15, 30, 60, 90, 120, 144)] public int TargetFrameRate { get; private set; }
    [field: SerializeField] public bool FullScreen { get; private set; } = true;

    [field: HorizontalLine("Language")]
    [field: SerializeField] public string Language { get; private set; }


    public void UpdateAudioSettings(float masterVolume, float musicVolume, float SoundVolume)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SoundsVolume = SoundVolume;
    }

    public void UpdateGraphicsSettings(bool vSync, int targetFrameRate, bool fullScreen)
    {
        VSync = vSync;
        TargetFrameRate = targetFrameRate;
        FullScreen = fullScreen;
    }

    public void UpdateLanguageSettings(string language)
    {
        Language = language;
    }

    public void UpdateFromOtherConfig(SettingsData otherConfig)
    {
        UpdateAudioSettings(otherConfig.MasterVolume, otherConfig.MusicVolume, otherConfig.SoundsVolume);
        UpdateGraphicsSettings(otherConfig.VSync, otherConfig.TargetFrameRate, otherConfig.FullScreen);
        UpdateLanguageSettings(otherConfig.Language);
    }
}
