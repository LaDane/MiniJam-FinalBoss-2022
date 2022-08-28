using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO SEFO;

    // Start is called before the first frame update
    void Start()
    {
        PlayFireSound();
    }

    private void PlayFireSound() {
        audioSource.Stop();
        audioSource.clip = SEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
