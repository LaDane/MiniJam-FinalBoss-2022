using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAlly : MonoBehaviour {
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private GameObject projectile;
    private Transform allyToHeal;

    private AIUnit aiUnit;
    private GameObject bolt;

    private void Start() {
        aiUnit = transform.GetComponentInParent<AIUnit>();
    }

    private void Update() {
        if (allyToHeal != null) {
            if (bolt != null) {
                bolt.GetComponent<EnemyProjectile>().endPos = allyToHeal;
            }
            Vector3 targetDirection = allyToHeal.position - transform.position;
            Quaternion spreadAngle = Quaternion.AngleAxis(aiUnit.attackAngleOffset, new Vector3(0, 1, 0));     // Offset look direction
            targetDirection = spreadAngle * targetDirection;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 4f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }


    public void Heal() {
        allyToHeal = AIManager.Instance.units[Random.Range(0, AIManager.Instance.units.Count)].transform;
        Vector3 direction = (allyToHeal.position - projectileSpawnPos.position).normalized;
        bolt = Instantiate(projectile, projectileSpawnPos.position, Quaternion.LookRotation(direction), null);
    }
}
