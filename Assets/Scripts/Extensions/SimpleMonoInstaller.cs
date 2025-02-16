using UnityEngine;
using Zenject;

public class SimpleMonoInstaller<T> : MonoInstaller where T : MonoService
{
    [SerializeField] private T service;

    public override void InstallBindings() => Container.Bind<T>().FromInstance(service).AsSingle().OnInstantiated<T>((ctx, instance) => instance.OnInstantiated());
    private void Reset() => service = GetComponent<T>();
}