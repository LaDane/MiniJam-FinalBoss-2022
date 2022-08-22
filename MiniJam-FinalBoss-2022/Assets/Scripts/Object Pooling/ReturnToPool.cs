using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour {

    public IObjectPool<GameObject> pool;

    private void Release() {
        pool.Release(gameObject);
    }
}
