using CustomInspector;
using UnityEngine;
using Zenject;

public class SimpleMonoInstaller<T> : MonoInstaller where T : MonoService
{
    [SelfFill(true), SerializeField] private T service;

    public override void InstallBindings() => Container.Bind<T>().FromInstance(service).AsSingle().OnInstantiated<T>((ctx, instance) => instance.Initialize());
    private void Reset() => service = GetComponent<T>();
}