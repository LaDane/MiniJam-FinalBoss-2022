using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject pauseScreen;

    void Start()
    {
        
    }

    public void OnAudioButtonPress() {

    }

    public void OnMusicButtonPress() {
        musicSource.mute =! musicSource.mute;
    }
    public void OnRestartBtnPress() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        UIManager.Instance.ButtonRestart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PlayerControls.Instance.controls.escapeButton) || Input.GetKeyDown(PlayerControls.Instance.controls.pButton)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        if (pauseScreen.activeSelf) {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        } else {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
