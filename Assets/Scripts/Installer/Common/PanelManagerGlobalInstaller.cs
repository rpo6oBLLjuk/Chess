using Zenject;

public class PanelManagerGlobalInstaller : Installer<PanelManagerGlobalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<PanelManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}