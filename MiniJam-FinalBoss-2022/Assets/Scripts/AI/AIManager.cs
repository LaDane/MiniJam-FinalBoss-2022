using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    
    private static AIManager _instance;
    public static AIManager Instance {
        get {
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    public Transform target;
    public List<AIUnit> units = new List<AIUnit>();
    public bool moveEnemies = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start() {
        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies() {
        while (moveEnemies) {
            if (!PlayerHealthManager.Instance.isAlive) {
                moveEnemies = false;
                break;
            }

            //yield return new WaitForSeconds(0.01f);
            yield return null;

            for (int i = 0; i < units.Count; i++) {
                //yield return new WaitForSeconds(0.01f);
                //yield return null;
                if (!units[i].isAlive) {
                    units.Remove(units[i]);
                    continue;
                }

                if (Vector3.Distance(units[i].transform.position, target.transform.position) > units[i].maxRange) {
                    units[i].SwitchStateMoving();

                    if (units[i].classType == ClassType.Tank || units[i].classType == ClassType.Rogue) {
                        units[i].MoveTo(new Vector3(
                            target.position.x + units[i].idealRange * Mathf.Cos(2 * Mathf.PI * i / units.Count),
                            target.position.y,
                            target.position.z + units[i].idealRange * Mathf.Sin(2 * Mathf.PI * i / units.Count)
                            ));
                    }
                    if (units[i].classType == ClassType.Healer || units[i].classType == ClassType.Mage || units[i].classType == ClassType.Warlock) {
                        Vector3 idealPos = Vector3.Lerp(target.position, units[i].transform.position, units[i].idealRange / (target.position - units[i].transform.position).magnitude);
                        Vector3 closePos = new Vector3(
                            idealPos.x + Random.Range(-3f, 3f),
                            idealPos.y,
                            idealPos.z + Random.Range(-3f, 3f));
                        units[i].MoveTo(closePos);
                    }
                }
            }
        }
    }
}