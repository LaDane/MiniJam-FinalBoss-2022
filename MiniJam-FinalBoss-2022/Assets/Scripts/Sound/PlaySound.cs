using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;

    public void SoundPlay() {
        audioSource.Stop();
        audioSource.clip = SEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
