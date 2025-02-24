using UnityEngine;
using Zenject;

public class MonoService : MonoBehaviour
{
    [Inject] protected DiContainer container;

    public virtual void OnInstantiated() { } //public because it is called externally during injection 
}
