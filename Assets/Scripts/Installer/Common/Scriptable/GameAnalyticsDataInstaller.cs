using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameAnalyticsDataInstaller", menuName = "Installlers/GameAnalyticsDataInstaller")]

public class GameAnalyticsDataInstaller : ScriptableObjectInstaller<GameAnalyticsDataInstaller>
{
    [SerializeField] private GameAnalyticsData currentData;

    public override void InstallBindings() => Container.Bind<GameAnalyticsData>().FromInstance(currentData).AsSingle();
}
