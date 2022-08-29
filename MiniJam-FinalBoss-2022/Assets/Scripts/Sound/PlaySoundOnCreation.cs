using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCreation : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;
    private void Start() {
        audioSource.Stop();
        audioSource.clip = SEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
