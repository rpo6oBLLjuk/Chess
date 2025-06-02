using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SkinDataInstaller", menuName = "Installlers/SkinDataInstaller")]
public class SkinDataInstaller : ScriptableObjectInstaller<SkinDataInstaller>
{
    [SerializeField] private SkinData currentData;

    public override void InstallBindings() => Container.Bind<SkinData>().FromInstance(currentData).AsSingle();
}