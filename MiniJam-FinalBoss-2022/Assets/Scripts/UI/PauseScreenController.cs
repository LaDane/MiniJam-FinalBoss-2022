using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource musicSource;

    void Start()
    {
        
    }

    public void OnAudioButtonPress() {

    }

    public void OnMusicButtonPress() {
        musicSource.mute =! musicSource.mute;
    }
    public void OnRestartBtnPress() {
        UIManager.Instance.ButtonRestart();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
