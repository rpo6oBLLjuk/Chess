using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbstractFactory
{
    public Transform container;
    public List<GameObject> pool;


    public virtual GameObject Get(GameObject prefab) => prefab;
    public virtual void Release(GameObject instance) => instance.SetActive(false);

    protected virtual GameObject Instantiate(GameObject prefab)
    {
        GameObject instance = UnityEngine.Object.Instantiate(prefab, container);
        pool.Add(instance);

        return instance;
    }
}
