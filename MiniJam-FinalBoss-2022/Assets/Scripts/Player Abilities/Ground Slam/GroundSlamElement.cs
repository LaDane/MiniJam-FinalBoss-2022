using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamElement : MonoBehaviour {

    [SerializeField] private Rigidbody rb;

    private Ability ability;
    private bool stopped;

    private void Start() {
        ability = GetComponentInParent<GroundSlam>().ability;
        rb.velocity = transform.forward * ability.speed;
        StartCoroutine(SlowDown());
    }

    private void FixedUpdate() {
        if (!stopped) {
            RaycastHit hit;
            Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(offsetPosition, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, PlayerAbilityManager.Instance.groundLayer)) {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    private IEnumerator SlowDown() {
        float time = 1;
        while (time > 0) {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, time);
            time -= ability.slowDownRate;
            //yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(Time.deltaTime * ability.lifeSpan);
            //yield return null;
        }
        stopped = true;
    }
}
