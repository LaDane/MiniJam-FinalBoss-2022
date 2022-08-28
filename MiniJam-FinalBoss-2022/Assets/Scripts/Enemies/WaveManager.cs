using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaveManager : MonoBehaviour {

    public int waveCounter = 0;

    [Header("Kill target")]
    public int kills = 0;
    public int killTarget1 = 20;
    public int killTarget2 = 40;
    public int killTarget3 = 60;
    public int killTarget4 = 80;

    [Header("Time Between Waves")]
    [SerializeField] private float timeBeforeFirstWave = 3f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float timeBetweenReduction = 0.1f;
    [SerializeField] private int reduceTimeEveryXWave = 1;
    [SerializeField] private float minTimeBetweenWaves = 2f;

    [Header("Amount of Groups")]
    [SerializeField] private int increaseGroupsEveryXWave = 3;
    [SerializeField] private int amountOfGroups = 1;

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    private int lastSpawnPosIndex = 0;

    //private int groupSize = 5;
    private float timeLeft;
    private float waitBetweenInstantiate = 0.2f;

    // Object pools
    private ObjectPool<GameObject> poolTank;
    private ObjectPool<GameObject> poolRogue;
    private ObjectPool<GameObject> poolWarlock;
    private ObjectPool<GameObject> poolMage;
    private ObjectPool<GameObject> poolHealer;

    private static WaveManager _instance;
    public static WaveManager Instance {
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

    private void Start() {
        timeLeft = timeBeforeFirstWave;

        poolTank = ObjectPoolManager.Instance.GetObjectPoolByName("PoolTank");
        poolRogue = ObjectPoolManager.Instance.GetObjectPoolByName("PoolRogue");
        poolWarlock = ObjectPoolManager.Instance.GetObjectPoolByName("PoolWarlock");
        poolMage = ObjectPoolManager.Instance.GetObjectPoolByName("PoolMage");
        poolHealer = ObjectPoolManager.Instance.GetObjectPoolByName("PoolHealer");
    }

    private void Update() {
        if (!PlayerHealthManager.Instance.isAlive) {
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            waveCounter++;

            // Decrease time between waves
            if (waveCounter % reduceTimeEveryXWave == 0) {
                timeBetweenWaves -= timeBetweenReduction;
                if (timeBetweenWaves < minTimeBetweenWaves) {
                    timeBetweenWaves = minTimeBetweenWaves;
                }
            }

            // Increase amount of groups spawned per wave
            if (waveCounter % increaseGroupsEveryXWave == 0) {
                amountOfGroups++;
            }

            timeLeft = timeBetweenWaves;

            StartCoroutine(SpawnGroup(GetRandomSpawnPos()));

            //Debug.Log("Wave = " + waveCounter + " | Time Between Waves = " + timeBetweenWaves + " | Amount of Groups = " + amountOfGroups);
        }
    }

    private int GetRandomSpawnPos() {
        int rand = Random.Range(0, spawnPositions.Count);
        while (rand == lastSpawnPosIndex) {
            rand = Random.Range(0, spawnPositions.Count);
        }
        lastSpawnPosIndex = rand;
        SpawnAudioHorn(spawnPositions[rand]);
        return rand;
    }

    private IEnumerator SpawnGroup(int spawnPosIndex) {
        for (int i = 0; i < amountOfGroups; i++) {
            PlaceEnemyUnit(poolTank, spawnPosIndex);
            yield return new WaitForSeconds(waitBetweenInstantiate);

            //PlaceEnemyUnit(poolRogue, spawnPosIndex);
            //yield return new WaitForSeconds(waitBetweenInstantiate);

            //PlaceEnemyUnit(poolWarlock, spawnPosIndex);
            //yield return new WaitForSeconds(waitBetweenInstantiate);

            PlaceEnemyUnit(poolMage, spawnPosIndex);
            yield return new WaitForSeconds(waitBetweenInstantiate);

            PlaceEnemyUnit(poolHealer, spawnPosIndex);
            yield return new WaitForSeconds(waitBetweenInstantiate);
        }
    }

    private void SpawnAudioHorn(Transform hornPosition) {
        hornPosition.GetComponent<PlaySound>().SoundPlay();
    }

    private void PlaceEnemyUnit(ObjectPool<GameObject> pool, int spawnPosIndex) {
        GameObject go = pool.Get();
        go.transform.position = GetSpawnPos(spawnPosIndex);
        go.GetComponent<AIUnit>().StartAIUnit();
    }

    private Vector3 GetSpawnPos(int spawnPosIndex) {
        Vector3 spawnPos = Random.insideUnitCircle * 4f;
        return spawnPositions[spawnPosIndex].position + spawnPos;
    }
}
