using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider sphereCollider;

    [HideInInspector] public Ability ability;

    private bool stillInHand = true;
    private float time = 0;
    private Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 endSize = new Vector3(1.5f, 1.5f, 1.5f);

    private Transform cameraTransform;
    private bool increaseVelocity = true;
    private bool onGround = false;

    private void Start() {
        for (int i = 0; i < PlayerAbilityManager.Instance.abilities.Length; i++) {
            if (PlayerAbilityManager.Instance.abilities[i].abilityName.Equals("Fireball")) {
                ability = PlayerAbilityManager.Instance.abilities[i];
            }
        }
        transform.localScale = startSize;
        cameraTransform = Camera.main.transform;
    }

    private void Update() {
        time += Time.deltaTime;
        if (time < ability.lifeSpan) {
            transform.position = PlayerAbilityManager.Instance.leftHandPoint.position;
            transform.localScale = Vector3.Lerp(startSize, endSize, time / ability.lifeSpan);
        }
        else {
            if (stillInHand) {
                stillInHand = false;
                rb.useGravity = true;
            }
            if (increaseVelocity) {
                Vector3 velocity = cameraTransform.forward * 1000 * ability.thrust * Time.deltaTime;
                rb.AddForce(velocity);
                rb.AddTorque(transform.right * 20 * Time.deltaTime, ForceMode.VelocityChange);
            }
            else {
                if (!onGround) {
                    onGround = true;
                    StartCoroutine(FallThroughGround());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!stillInHand && increaseVelocity) {
            increaseVelocity = false;
        }
    }

    private IEnumerator FallThroughGround() {
        float timeElapsed = 0;
        while(timeElapsed < ability.destroyDelay) {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        sphereCollider.enabled = false;
        //timeElapsed = 0;
        //while (timeElapsed < ability.destroyDelay) {
        //    timeElapsed += Time.deltaTime;
        //    yield return null;
        //}
        Destroy(gameObject, ability.destroyDelay);
    }

    //private IEnumerator FallThroughGround() {
    //    float timeElapsed = 0;
    //    Vector3 startPos = transform.position;
    //    Vector3 endPos = new Vector3(startPos.x, startPos.y - 9f, startPos.z);
    //    while (timeElapsed < ability.destroyDelay) {
    //        transform.position = Vector3.Lerp(startPos, endPos, (timeElapsed - 1f) / ability.destroyDelay);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    Destroy(gameObject);
    //}
}
