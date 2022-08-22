using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour {
    
    [HideInInspector] public Ability ability;

    [SerializeField] private Rigidbody impactColliderRB;

    private float time;

    private void Start() {
        for (int i = 0; i < PlayerAbilityManager.Instance.abilities.Length; i++) {
            if (PlayerAbilityManager.Instance.abilities[i].abilityName.Equals("Normal Attack")) {
                ability = PlayerAbilityManager.Instance.abilities[i];
            }
        }
    }

    private void Update() {
        time += Time.deltaTime;
        if (time < ability.lifeSpan) {
            //Vector3 velocity = new Vector3(-ability.thrust * 1000 * Time.deltaTime, 0, 0);
            impactColliderRB.AddForce(1000 * ability.thrust * -transform.right * Time.deltaTime);
        }
        else {
            Destroy(gameObject);
        }
    }
}
