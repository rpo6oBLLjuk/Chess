using UnityEngine;
using Zenject;

public class MonoService : MonoBehaviour, IInitializable
{
    [Inject] protected DiContainer container;

    public virtual void Initialize() { } //public because it is called externally during injection
}
