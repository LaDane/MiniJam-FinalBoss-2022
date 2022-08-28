using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour {

    [SerializeField] private BoxCollider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private GameObject armature;
    [SerializeField] private Animator animator;
    [SerializeField] private AIUnit aIUnit;

    [Header("Sound")]
    public AudioSource impactAudioSource;
    public AudioSource screamAudioSource;
    [SerializeField] private SoundEffectSO impactSFXSO;
    [SerializeField] private SoundEffectSO screamSFXSO;
    
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    private void Awake() {
        GetRagdollParts();
        RagdollModeOff();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("RagdollActivator")) {
            aIUnit.isAlive = false;
            RagdollModeOn();

            if (impactAudioSource != null && impactSFXSO != null && screamAudioSource != null && screamSFXSO != null) {
                PlayImpactSound();
                if (Random.Range(0, 100) < 15) {
                    //Debug.Log(Random.Range(0, 100));
                    PlayScreamSound();   
                }
            }
        }
    }

    private void PlayImpactSound() {
        impactAudioSource.Stop();
        impactAudioSource.clip = impactSFXSO.GetSound(impactAudioSource);
        impactAudioSource.Play();
    }

    private void PlayScreamSound() {
        screamAudioSource.Stop();
        screamAudioSource.clip = screamSFXSO.GetSound(screamAudioSource);
        screamAudioSource.Play();
    }

    public void StopAllSounds() {
        screamAudioSource.Stop();
        impactAudioSource.Stop();
    }


    private void GetRagdollParts() {
        ragdollColliders = armature.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = armature.GetComponentsInChildren<Rigidbody>();
    }


    public void RagdollModeOn() {
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

    public void RagdollModeOff() {
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

    public void DissapearThroughGround() {
        foreach (Collider c in ragdollColliders) {
            c.enabled = false;
        }
        foreach (Rigidbody r in ragdollRigidbodies) {
            r.isKinematic = false;
        }
        mainRigidbody.isKinematic = false;
        mainRigidbody.useGravity = true;
    }
}
