using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsDataInstaller", menuName = "Installlers/SettingsDataInstaller")]
public class SettingsDataInstaller : ScriptableObjectInstaller<SettingsDataInstaller>
{
    [SerializeField] private SettingsData currentData;

    public override void InstallBindings()
    {
        Container.Bind<SettingsData>().FromInstance(currentData).AsSingle();
        currentData.ApplyGraphicsSettings();
    }
}
