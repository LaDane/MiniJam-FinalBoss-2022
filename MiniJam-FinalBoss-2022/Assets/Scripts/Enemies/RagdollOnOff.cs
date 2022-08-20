using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour {

    [SerializeField] private BoxCollider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private GameObject armature;
    [SerializeField] private Animator animator;
    [SerializeField] private AIUnit aIUnit;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    private void Start() {
        GetRagdollParts();
        RagdollModeOff();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("RagdollActivator")) {
            aIUnit.isAlive = false;
            RagdollModeOn();
            //StartCoroutine(DissapearThroughGround());
        }
    }

    private void GetRagdollParts() {
        ragdollColliders = armature.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = armature.GetComponentsInChildren<Rigidbody>();
    }

    private void RagdollModeOn() {
        animator.enabled = false;

        foreach (Collider c in ragdollColliders) {
            c.enabled = true;
        }
        foreach (Rigidbody r in ragdollRigidbodies) {
            r.isKinematic = false;
        }

        mainCollider.enabled = false;
        mainRigidbody.isKinematic = true;
    }

    private void RagdollModeOff() {
        foreach (Collider c in ragdollColliders) {
            c.enabled = false;
        }
        foreach (Rigidbody r in ragdollRigidbodies) {
            r.isKinematic = true;
        }

        animator.enabled = true;
        mainCollider.enabled = true;
        mainRigidbody.isKinematic = false;
    }

    public IEnumerator DissapearThroughGround() {
        foreach (Collider c in ragdollColliders) {
            c.enabled = false;
        }
        foreach (Rigidbody r in ragdollRigidbodies) {
            r.isKinematic = false;
        }
        mainRigidbody.isKinematic = false;
        mainRigidbody.useGravity = true;

        yield return new WaitForSeconds(1f);
        if (transform.position.y < 10f) {
            Destroy(gameObject);
        }
    }
}
