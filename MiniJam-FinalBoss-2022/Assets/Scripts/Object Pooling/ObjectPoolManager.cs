using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour {

    //[SerializeField] private OPool[] poolTypes;
    
    public ObjectPool<GameObject>[] pools;

    [SerializeField] private ObjectPoolDict[] poolTypes;

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
                poolTypes[i].pool.CreatePoolObject,
                poolTypes[i].pool.OnTakeFromPool,
                poolTypes[i].pool.OnReturnToPool,
                poolTypes[i].pool.OnDestroyPoolObject,
                true,
                poolTypes[i].pool.poolStartSize
                );
            for (int j = 0; j < poolTypes[i].pool.poolStartSize; j++) {
                GameObject go = poolTypes[i].pool.CreatePoolObject();
                go.GetComponent<ReturnToPool>().pool = pools[i];
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
