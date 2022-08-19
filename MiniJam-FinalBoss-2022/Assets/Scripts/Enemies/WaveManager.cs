using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public int waveCounter = 0;

    [Header("Time Between Waves")]
    [SerializeField] private float timeBeforeFirstWave = 3f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float timeBetweenReduction = 0.1f;
    [SerializeField] private int reduceTimeEveryXWave = 1;
    [SerializeField] private float minTimeBetweenWaves = 2f;

    [Header("Amount of Groups")]
    [SerializeField] private int increaseGroupsEveryXWave = 3;
    private int amountOfGroups = 1;

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    private int lastSpawnPosIndex = 0;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> tankPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> healerPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> magePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> warlockPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> roguePrefabs = new List<GameObject>();

    //private int groupSize = 5;
    private float timeLeft;
    private float waitBetweenInstantiate = 0.2f;

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
        if (spawnPositions.Count == 0)
            Debug.LogWarning("Wave Manager is missing spawn positions!");
        if (tankPrefabs.Count == 0)
            Debug.LogWarning("Wave Manager is missing tank prefabs!");
        if (healerPrefabs.Count == 0)
            Debug.LogWarning("Wave Manager is missing healer prefabs!");
        if (magePrefabs.Count == 0)
            Debug.LogWarning("Wave Manager is missing mage prefabs!");
        if (warlockPrefabs.Count == 0)
            Debug.LogWarning("Wave Manager is missing warlock prefabs!");
        if (roguePrefabs.Count == 0)
            Debug.LogWarning("Wave Manager is missing rogue prefabs!");

        timeLeft = timeBeforeFirstWave;
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

            StartCoroutine(InstantiateGroup(GetRandomSpawnPos()));

            Debug.Log("Wave = " + waveCounter + " | Time Between Waves = " + timeBetweenWaves + " | Amount of Groups = " + amountOfGroups);
        }
    }

    private int GetRandomSpawnPos() {
        int rand = Random.Range(0, spawnPositions.Count);
        while (rand == lastSpawnPosIndex)
            rand = Random.Range(0, spawnPositions.Count);
        lastSpawnPosIndex = rand;
        return rand;
    }

    private IEnumerator InstantiateGroup(int randomInt) {
        for (int i = 0; i < amountOfGroups; i++) {
            yield return new WaitForSeconds(waitBetweenInstantiate);
            Instantiate(tankPrefabs[Random.Range(0, tankPrefabs.Count)], GetSpawnPos(randomInt), Quaternion.identity);
            
            yield return new WaitForSeconds(waitBetweenInstantiate);
            Instantiate(healerPrefabs[Random.Range(0, healerPrefabs.Count)], GetSpawnPos(randomInt), Quaternion.identity);
            
            yield return new WaitForSeconds(waitBetweenInstantiate);
            Instantiate(magePrefabs[Random.Range(0, magePrefabs.Count)], GetSpawnPos(randomInt), Quaternion.identity);
            
            yield return new WaitForSeconds(waitBetweenInstantiate);
            Instantiate(warlockPrefabs[Random.Range(0, warlockPrefabs.Count)], GetSpawnPos(randomInt), Quaternion.identity);
            
            yield return new WaitForSeconds(waitBetweenInstantiate);
            Instantiate(roguePrefabs[Random.Range(0, roguePrefabs.Count)], GetSpawnPos(randomInt), Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPos(int randomInt) {
        Vector3 spawnPos = Random.insideUnitCircle * 4.5f;
        return spawnPositions[randomInt].position + spawnPos;
    }
}
