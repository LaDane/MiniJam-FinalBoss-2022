using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [Header("UI Objects")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image killBar;
    [SerializeField] private Text killText;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Ability Fetchers")]
    [SerializeField] private GameObject ability1;
    [SerializeField] private GameObject ability2;
    [SerializeField] private GameObject ability3;
    //[SerializeField] private GameObject ability4;

    [Header("Game Over Stats")]
    [SerializeField] private Text killStats;
    [SerializeField] private Text timeStats;
    [SerializeField] private Text waveStats;

    int killPhaseStart = 0;
    int killTarget = 0;
    int stage = 0;

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
            UpdateKillCount();
        }
        else {
            if (!displayingGameOverScreen) {
                displayingGameOverScreen = true;
                gameOverScreen.SetActive(true);
                string killStatsString = AIManager.Instance.killCount.ToString();
                string waveStatsString = WaveManager.Instance.waveCounter.ToString();
                killStats.text = "Minions killed: " + killStatsString;
                waveStats.text = "Waves reached: " + waveStatsString;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = (float)PlayerHealthManager.Instance.playerHP / (float)PlayerHealthManager.Instance.playerMaxHP;
    }

    private void UpdateKillCount() {


        int kills = AIManager.Instance.killCount;
   
        int killTarget1 = WaveManager.Instance.killTarget1;
        int killTarget2 = WaveManager.Instance.killTarget2;
        int killTarget3 = WaveManager.Instance.killTarget3;
        int killTarget4 = WaveManager.Instance.killTarget4;

        string killsString;

        switch (stage) {
            case 0:
                PlayerAbilityManager.Instance.abilities[0].isActive = true;
                killTarget = killTarget1;
                if(kills >= killTarget) {
                    PlayerHealthManager.Instance.playerMaxHP = 200;
                    PlayerHealthManager.Instance.playerHP = PlayerHealthManager.Instance.playerMaxHP;
                    PlayerHealthManager.Instance.regenAmount = 3;
                    ability1.SetActive(true);
                    killPhaseStart = kills;
                    killBar.fillAmount = 0f;
                    PlayerAbilityManager.Instance.abilities[1].isActive = true;
                    stage = 1;
                }
                break;
            case 1:
                killTarget = killTarget2;
                if (kills >= killTarget) {
                    PlayerHealthManager.Instance.playerMaxHP = 400;
                    PlayerHealthManager.Instance.playerHP = PlayerHealthManager.Instance.playerMaxHP;
                    PlayerHealthManager.Instance.regenAmount = 5;
                    ability2.SetActive(true);
                    killPhaseStart = kills;
                    killBar.fillAmount = 0f;
                    PlayerAbilityManager.Instance.abilities[2].isActive = true;
                    stage = 2;
                }
                break;
            case 2:
                killTarget = killTarget3;
                if (kills >= killTarget) {
                    PlayerHealthManager.Instance.playerMaxHP = 1200;
                    PlayerHealthManager.Instance.playerHP = PlayerHealthManager.Instance.playerMaxHP;
                    PlayerHealthManager.Instance.regenAmount = 7;
                    ability3.SetActive(true);
                    killPhaseStart = kills;
                    PlayerAbilityManager.Instance.abilities[3].isActive = true;
                    //killBar.fillAmount = 0f;
                    stage = 3;
                }
                break;
            case 3:
                /*
                killTarget = killTarget4;
                if (kills >= killTarget) {
                    ability4.SetActive(true);
                    killPhaseStart = kills;
                    PlayerAbilityManager.Instance.abilities[4].isActive = true;
                    stage = 4;
                }*/
                break;
            case 4:
                break;
        }

        for(int i = 0; i < PlayerAbilityManager.Instance.abilities.Length-1; i++) {
            if (PlayerAbilityManager.Instance.abilities[i].remainingCooldown == PlayerAbilityManager.Instance.abilities[i].cooldown) {
                switch (PlayerAbilityManager.Instance.abilities[i].abilityName) {
                    case "Fireball":
                        ability1.GetComponent<Image>().fillAmount = 0f;
                        break;
                    case "Ground Slam":
                        ability2.GetComponent<Image>().fillAmount = 0f;
                        break;
                    case "Hammer Quake":
                        ability3.GetComponent<Image>().fillAmount = 0f;
                        break;
  /*                  case "Grab Throw":
                        ability3.GetComponent<Image>().fillAmount = 0f;
                        break;*/
 
                }

            }
            
            if(PlayerAbilityManager.Instance.abilities[i].remainingCooldown > 0) {
                switch (PlayerAbilityManager.Instance.abilities[i].abilityName) {
                    case "Fireball":
                        ability1.GetComponent<Image>().fillAmount = 1 - ((float)PlayerAbilityManager.Instance.abilities[i].remainingCooldown / (float)PlayerAbilityManager.Instance.abilities[i].cooldown);
                        break;
                    case "Ground Slam":
                        ability2.GetComponent<Image>().fillAmount = 1 - ((float)PlayerAbilityManager.Instance.abilities[i].remainingCooldown / (float)PlayerAbilityManager.Instance.abilities[i].cooldown);
                        break;
                    case "Hammer Quake":
                        ability3.GetComponent<Image>().fillAmount = 1 - ((float)PlayerAbilityManager.Instance.abilities[i].remainingCooldown / (float)PlayerAbilityManager.Instance.abilities[i].cooldown);
                        break;
        /*            case "Grab Throw":
                        ability3.GetComponent<Image>().fillAmount = 1 - ((float)PlayerAbilityManager.Instance.abilities[i].remainingCooldown / (float)PlayerAbilityManager.Instance.abilities[i].cooldown);
                        break;*/

                }
            }
        }
        

        killBar.fillAmount = ((float)kills - (float)killPhaseStart) / ((float)killTarget - (float)killPhaseStart);

        killsString = kills.ToString();
        killText.text = killsString;
    }

    public void ButtonRestart() {
        Debug.Log("Restart");
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonMainMenu() {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
