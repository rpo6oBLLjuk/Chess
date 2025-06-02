using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameDataInstaller", menuName = "Installlers/GameDataInstaller")]
public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
{
    [SerializeField] private GameData currentData;

    public override void InstallBindings() => Container.Bind<GameData>().FromInstance(currentData).AsSingle();
}