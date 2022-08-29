using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAlly : MonoBehaviour {
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private GameObject projectile;
    private Transform allyToHeal;

    private AIUnit aiUnit;
    private GameObject bolt;
    private EnemyProjectile enemyProjectile;

    private void Start() {
        aiUnit = transform.GetComponentInParent<AIUnit>();
    }

    private void Update() {
        if (allyToHeal != null && aiUnit.isAttacking) {
            Vector3 targetDirection = allyToHeal.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(aiUnit.transform.forward, targetDirection, 4f * Time.deltaTime, 0.0f);
            aiUnit.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }


    public void Heal() {
        Vector3 direction = (allyToHeal.position - projectileSpawnPos.position).normalized;
        bolt = Instantiate(projectile, projectileSpawnPos.position, Quaternion.LookRotation(direction), null);
        bolt.GetComponent<EnemyProjectile>().endPos = allyToHeal.GetComponent<AIUnit>().ragdollHips;
    }

    public void RotateTowardsAlly() {
        allyToHeal = AIManager.Instance.units[Random.Range(0, AIManager.Instance.units.Count)].transform;
    }
}
