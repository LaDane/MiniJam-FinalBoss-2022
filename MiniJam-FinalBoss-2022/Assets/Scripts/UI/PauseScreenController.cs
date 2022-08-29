using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioMixer audioMixer;

    private bool isMusicMuted = false;
    private bool isSoundMuted = false;

    void Start()
    {
        
    }

    public void OnAudioButtonPress() {
        if (isSoundMuted) {
            audioMixer.SetFloat("soundVolume", 0f);
            isSoundMuted = false;
        } else {
            audioMixer.SetFloat("soundVolume", -80f);
            isSoundMuted = true;
        }
    }

    public void OnMusicButtonPress() {
        if (isMusicMuted) {
            audioMixer.SetFloat("musicVolume", 0f);
            isMusicMuted = false;
        } else {
            audioMixer.SetFloat("musicVolume", -80f);
            isMusicMuted = true;
        }
    }
    public void OnRestartBtnPress() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        UIManager.Instance.ButtonRestart();
    }

    public void onMainMenuButtonPress() {

        SceneManager.LoadScene("MainMenu");
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
