using UnityEngine;
using Zenject;

public class MonoService : MonoBehaviour, IInitializable
{
    [Inject] protected DiContainer container;

    public virtual void Initialize() { } //called externally during injection
}
