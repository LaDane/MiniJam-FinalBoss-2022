using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerQuake : MonoBehaviour {

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody boxImpactColliderRB;
    [SerializeField] private Rigidbody sphereImpactColliderRB;
    [SerializeField] private GameObject groundExplosion;
    [SerializeField] private Transform flames;
    [SerializeField] private int groundExplosionsToSpawn = 5;

    [HideInInspector] public Ability ability;

    private float time = 0;
    private bool spawnedExplosion = false;
    private float velocitySpeedMultiplaier = 300f;

    private void Start() {
        for (int i = 0; i < PlayerAbilityManager.Instance.abilities.Length; i++) {
            if (PlayerAbilityManager.Instance.abilities[i].abilityName.Equals("Hammer Quake")) {
                ability = PlayerAbilityManager.Instance.abilities[i];
            }
        }
        sphereImpactColliderRB.transform.localScale = new Vector3(ability.size, ability.size, ability.size);
        sphereImpactColliderRB.transform.localPosition = new Vector3(0, -ability.size, 0);
    }

    

    private void FixedUpdate() {
        time += Time.deltaTime;
        if (time < ability.lifeSpan) {
            RaycastHit hit;
            Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(offsetPosition, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, PlayerAbilityManager.Instance.groundLayer)) {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            rb.velocity = transform.forward * ability.speed * velocitySpeedMultiplaier * Time.deltaTime;
            boxImpactColliderRB.velocity = transform.forward * ability.speed * velocitySpeedMultiplaier * Time.deltaTime;
        }
        else {
            rb.velocity = Vector3.zero;
            boxImpactColliderRB.velocity = Vector3.zero;
            boxImpactColliderRB.gameObject.SetActive(false);
            if (!spawnedExplosion) {
                spawnedExplosion = true;
                for (int i = 0; i < groundExplosionsToSpawn; i++) {
                    Vector3 pointInCircle = new Vector3(
                        transform.position.x + 10f * Mathf.Cos(2 * Mathf.PI * i / groundExplosionsToSpawn),
                        transform.position.y,
                        transform.position.z + 10f * Mathf.Sin(2 * Mathf.PI * i / groundExplosionsToSpawn)
                        );
                    Vector3 direction = (pointInCircle - transform.position).normalized;
                    
                    GameObject groundExplosionGO = Instantiate(groundExplosion, transform.position, Quaternion.LookRotation(direction), transform);
                    Rigidbody groundExplosionRB = groundExplosionGO.GetComponent<Rigidbody>();
                    groundExplosionRB.velocity = direction * ability.speed;

                    StartCoroutine(SlowDown(groundExplosionRB));
                    StartCoroutine(MoveFlamesDown());

                    Destroy(gameObject, ability.destroyDelay);
                }
            }
            if (sphereImpactColliderRB != null) {
                if (sphereImpactColliderRB.transform.localPosition.y < 2f) {
                    sphereImpactColliderRB.AddForce(0, ability.thrust * 1000 * Time.deltaTime, 0);
                }
                else {
                    Destroy(sphereImpactColliderRB.gameObject);
                }
            }
        }
    }

    private IEnumerator SlowDown(Rigidbody rb) {
        float time = ability.lifeSpan + 0.5f;
        while (time > 0) {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, time);
            time -= ability.slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator MoveFlamesDown() {
        float timeElapsed = 0;
        Vector3 startPos = flames.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y - 9f, startPos.z);
        while (timeElapsed < ability.lifeSpan + 2f) {
            if (timeElapsed > 1f) {
                flames.position = Vector3.Lerp(startPos, endPos, (timeElapsed -1f) / ability.lifeSpan);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
