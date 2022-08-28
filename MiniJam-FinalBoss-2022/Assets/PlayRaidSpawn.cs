using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRaidSpawn : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;

    private void PlayFireSound() {
        audioSource.Stop();
        audioSource.clip = SEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
