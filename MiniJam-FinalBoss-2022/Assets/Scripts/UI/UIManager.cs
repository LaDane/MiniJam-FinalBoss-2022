using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] private Image healthBar;
    [SerializeField] private Image killBar;
    [SerializeField] private GameObject gameOverScreen;

    private bool displayingGameOverScreen = false;

    private static UIManager _instance;
    public static UIManager Instance {
        get {
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start() {
        gameOverScreen.SetActive(false);
    }

    private void Update() {
        if (PlayerHealthManager.Instance.isAlive) {
            UpdateHealthBar();
        }
        else {
            if (!displayingGameOverScreen) {
                displayingGameOverScreen = true;
                gameOverScreen.SetActive(true);
            }
        }
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = (float)PlayerHealthManager.Instance.playerHP / (float)PlayerHealthManager.Instance.playerMaxHP;
    }

    public void ButtonRestart() {
        Debug.Log("Restart");
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonMainMenu() {
        Debug.Log("Main Menu");
    }
}
