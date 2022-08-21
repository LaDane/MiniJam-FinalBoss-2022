using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

    private ClassType classType;
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private GameObject projectile;
    

    private void Start() {
        classType = transform.GetComponentInParent<AIUnit>().classType;
    }

    public void DoDamage() {
        if ((classType == ClassType.Mage && projectileSpawnPos != null) || (classType == ClassType.Warlock && projectileSpawnPos != null)) {
            Vector3 direction = (PlayerAbilityManager.Instance.playerCenterPoint.position - projectileSpawnPos.position).normalized;
            Instantiate(projectile, projectileSpawnPos.position, Quaternion.LookRotation(direction), null);
        }
        PlayerHealthManager.Instance.DamagePlayer(classType);
    }
}
