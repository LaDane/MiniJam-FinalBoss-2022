using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    [Header("Player Stats")]
    public bool isAlive = true;
    public int playerMaxHP = 1000;
    public int playerHP = 1000;

    [Header("Damage Types")]
    [SerializeField] private int tankDmg = 4;
    [SerializeField] private int healerDmg = 0;
    [SerializeField] private int mageDmg = 2;
    [SerializeField] private int warlockDmg = 2;
    [SerializeField] private int rogueDmg = 3;

    [Header("Regeneration")]
    public int regenAmount = 5;

    private static PlayerHealthManager _instance;
    public static PlayerHealthManager Instance {
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
        playerHP = playerMaxHP;
    }

    private void Update() {
        CheckIfAlive();
    }

    private void CheckIfAlive() {
        if (playerHP <= 0) {
            isAlive = false;
        }
    }

    public void DamagePlayer(ClassType classType) {
        if (isAlive) {
            switch (classType) {
                case ClassType.Tank: playerHP -= tankDmg; break;
                case ClassType.Healer: playerHP -= healerDmg; break;
                case ClassType.Mage: playerHP -= mageDmg; break;
                case ClassType.Warlock: playerHP -= warlockDmg; break;
                case ClassType.Rogue: playerHP -= rogueDmg; break;
            }
        }
    }
}
