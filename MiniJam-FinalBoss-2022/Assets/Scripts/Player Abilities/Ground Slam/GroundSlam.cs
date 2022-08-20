using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour {

    [SerializeField] private GameObject groundSlamElement;
    [SerializeField] private int elementsToSpawn = 10;
    [SerializeField] private Rigidbody impactColliderRB;

    [HideInInspector] public Ability ability;

    private float dirOffset = 10f;
    

    private void Start() {
        for (int i = 0; i < PlayerAbilityManager.Instance.abilities.Length; i++) {
            if (PlayerAbilityManager.Instance.abilities[i].abilityName.Equals("Ground Slam")) {
                ability = PlayerAbilityManager.Instance.abilities[i];
            }
        }

        for (int i = 0; i < elementsToSpawn; i++) {
            Vector3 pointInCircle = new Vector3(
                transform.position.x + dirOffset * Mathf.Cos(2 * Mathf.PI * i / elementsToSpawn),
                transform.position.y,
                transform.position.z + dirOffset * Mathf.Sin(2 * Mathf.PI * i / elementsToSpawn)
                );
            Vector3 direction = (pointInCircle - transform.position).normalized;
            Instantiate(groundSlamElement, transform.position, Quaternion.LookRotation(direction), transform);
        }

        impactColliderRB.transform.localScale = new Vector3(ability.size, ability.size, ability.size);
        impactColliderRB.transform.localPosition = new Vector3(0, -ability.size, 0);

        Destroy(gameObject, ability.destroyDelay);          // Destroy after destroy delay time
    }

    private void Update() {
        if (impactColliderRB != null) {
            if (impactColliderRB.transform.localPosition.y < 2f) {
                impactColliderRB.AddForce(0, ability.thrust * 1000 * Time.deltaTime, 0);
            }
            else {
                Destroy(impactColliderRB.gameObject);
            }
        }
    }
}
