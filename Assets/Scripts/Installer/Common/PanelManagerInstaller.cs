using CustomInspector;
using Zenject;

public class PanelManagerInstaller : MonoInstaller
{
    [SelfFill] PanelManager _panelManager;

    public override void InstallBindings()
    {
        Container.Unbind<PanelManager>();
        Container.Bind<PanelManager>().FromInstance(_panelManager).AsSingle();
    }
}
