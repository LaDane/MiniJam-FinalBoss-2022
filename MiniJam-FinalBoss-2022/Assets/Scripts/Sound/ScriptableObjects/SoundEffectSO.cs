using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Sound", order = 1)]
public class SoundEffectSO : ScriptableObject {
    public enum ClipPlayOrder {
        random,
        inOrder,
        reverse
    }

    public AudioClip[] clips;
    [SerializeField] public ClipPlayOrder clipPlayOrder;
    [Range(0, 1)] public float volume = 0.5f;
    [Range(0, 3)] public float pitch = 1;
    public bool is3dSound = true;

    [Header("Random Volume")]
    public bool randomVolume = false;
    public Vector2 volumeRange = new Vector2(0, 1);

    [Header("Random Pitch")]
    public bool randomPitch = false;
    public Vector2 pitchRange = new Vector2(0, 3);

    private int clipIndex = 0;

    private void Awake() {
        switch(clipPlayOrder) {
            case ClipPlayOrder.random: clipIndex = GetRandomClipIndex(); break;
            case ClipPlayOrder.inOrder: clipIndex = 0; break;
            case ClipPlayOrder.reverse: clipIndex = clips.Length - 1; break;
        }
    }

    public AudioClip GetSound(AudioSource audioSource) {
        if (clips.Length == 0) {
            Debug.LogWarning($"Missing sound clips for {this.name}");
            return null;
        }
        if (audioSource == null) {
            Debug.LogWarning($"An audio source is required to to get a sound for {this.name}");
            return null;
        }
        
        if (randomVolume) {
            audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);
        }
        else {
            audioSource.volume = volume;
        }
        
        if (randomPitch) {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        }
        else {
            audioSource.pitch = pitch;
        }

        audioSource.spatialBlend = is3dSound ? 1 : 0;
        return GetClipBasedOnOrder(clipIndex, false);
    }

    private AudioClip GetClipBasedOnOrder(int cIndex, bool isPreviewer) {
        AudioClip chosenClip = null;
        switch(clipPlayOrder) {
            case ClipPlayOrder.random: 
                chosenClip = clips[GetRandomClipIndex()];
                break;
            case ClipPlayOrder.inOrder:
                chosenClip = clips[cIndex];
                clipIndex++;
                if (clipIndex >= clips.Length) {
                    clipIndex = 0;
                }
                break;
            case ClipPlayOrder.reverse:
                chosenClip = clips[cIndex];
                clipIndex--;
                if (clipIndex < 0) {
                    clipIndex = clips.Length - 1;
                }
                break;
        }
        return chosenClip;
    }

    private int GetRandomClipIndex() {
        return Random.Range(0, clips.Length);
    }
}
