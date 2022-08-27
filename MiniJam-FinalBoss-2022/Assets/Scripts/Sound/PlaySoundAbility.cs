using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAbility : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundEffectSO FireballSEFO;
    [SerializeField] private SoundEffectSO GroundslamSEFO;
    [SerializeField] private SoundEffectSO HammerquakeSEFO;

    public void SoundPlayFireball() {
        audioSource.Stop();
        audioSource.clip = FireballSEFO.GetSound(audioSource);
        audioSource.Play();
    }

    public void SoundPlayGroundslam() {
        audioSource.Stop();
        audioSource.clip = GroundslamSEFO.GetSound(audioSource);
        audioSource.Play();
    }

    public void SoundPlayHammerquake() {
        audioSource.Stop();
        audioSource.clip = HammerquakeSEFO.GetSound(audioSource);
        audioSource.Play();
    }
}
