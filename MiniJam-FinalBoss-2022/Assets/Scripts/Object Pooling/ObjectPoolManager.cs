using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour {

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int startPoolSize;

    [HideInInspector] public IObjectPool<GameObject> pool;

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
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    //private void Start() {
    //    pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true);
    //}

    private void CreatePooledItem() {

    }
}
