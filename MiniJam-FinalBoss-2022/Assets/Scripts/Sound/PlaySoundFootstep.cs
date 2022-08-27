using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundFootstep : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;
    [SerializeField] private Animator animator;

    // Forward and backwards sound
    public void PlayFootstepVelocityZ() {
        PlayFootstep();
    }

    // Sideways
    public void PlayFootstepVelocityX() {
        if (animator.GetFloat("Velocity Z") == 0) {
            PlayFootstep();
        }
    }

    private void PlayFootstep() {
        audioSource.Stop();
        audioSource.clip = SEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
