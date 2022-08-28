using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundTaunt : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;

    public void SoundPlay() {
        if (!audioSource.isPlaying) {
            audioSource.clip = SEFO.GetSound(audioSource);
            audioSource.Play();
        }
    }
}
