using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour {

    [SerializeField] private OPool poolScriptableObject;
    public IObjectPool<GameObject> pool;

    private void Start() {
        if (pool == null) {
            pool = ObjectPoolManager.Instance.GetObjectPoolByName(poolScriptableObject.name);
        }
    }

    public void Release() {
        if (pool != null) {
            pool.Release(gameObject);
        }
        else {
            Debug.LogError(gameObject.name + " has no pool to be released to!");
        }
    }
}
