using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectPools/OPool", order = 1)]
public class OPool : ScriptableObject {

    public GameObject poolObject;
    public int poolStartSize;

    public virtual GameObject CreatePoolObject() {
        GameObject go = Instantiate(poolObject);
        go.AddComponent<ReturnToPool>();
        return go;
    }

    public virtual void OnReturnToPool(GameObject go) {
        go.SetActive(false);
    }

    public virtual void OnTakeFromPool(GameObject go) {
        go.SetActive(true);
    }

    public virtual void OnDestroyPoolObject(GameObject go) {
        Destroy(go);
    }
}
