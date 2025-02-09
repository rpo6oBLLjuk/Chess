using UnityEngine;
using Zenject;

public class SimpleMonoInstaller<T> : MonoInstaller where T : Component
{
    [SerializeField] private T service;

    public override void InstallBindings() => Container.Bind<T>().FromInstance(service).AsSingle();
    private void Reset() => service = GetComponent<T>();
}