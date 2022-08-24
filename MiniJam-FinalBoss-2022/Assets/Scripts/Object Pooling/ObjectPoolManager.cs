using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour {

    //[SerializeField] private OPool[] poolTypes;
    
    public ObjectPool<GameObject>[] pools;

    [SerializeField] private OPool[] poolTypes;
    //[SerializeField] private ObjectPoolDict[] poolTypes;

    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance {
        get {
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    private void Awake() {
        pools = new ObjectPool<GameObject>[poolTypes.Length];
        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new ObjectPool<GameObject>(
                poolTypes[i].CreatePoolObject,
                poolTypes[i].OnTakeFromPool,
                poolTypes[i].OnReturnToPool,
                poolTypes[i].OnDestroyPoolObject,
                true,
                poolTypes[i].poolStartSize
                );
            for (int j = 0; j < poolTypes[i].poolStartSize; j++) {
                GameObject go = poolTypes[i].CreatePoolObject();
                pools[i].Release(go);
            }
        }
        
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public ObjectPool<GameObject> GetObjectPoolByName(string poolName) {
        for (int i = 0; i < poolTypes.Length; i++) {
            if (poolTypes[i].name.Equals(poolName)) {
                return pools[i];
            }
        }

        Debug.LogError("No object pool with name '" + poolName + "' exists!");
        return null;
    }
}
