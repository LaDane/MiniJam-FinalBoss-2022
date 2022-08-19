using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour {

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Ability[] abilities;

    private bool isHoldingEnemy = false;

    private static PlayerAbilityManager _instance;
    public static PlayerAbilityManager Instance {
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
        AssignAbilityButtons();
    }

    private void Update() {
        if (!PlayerHealthManager.Instance.isAlive) {
            return;
        }
        for (int i = 0; i < abilities.Length; i++) {

            // Calculate ability cooldown
            abilities[i].remainingCooldown -= Time.deltaTime;
            if (abilities[i].remainingCooldown <= 0) {
                abilities[i].remainingCooldown = 0;

                // Play ability animation on key down
                if (Input.GetKeyDown(abilities[i].activationBtn)) {

                    if (abilities[i].animationName.Equals("Grab|Throw")) {
                        if (!isHoldingEnemy)
                            playerAnimator.Play("Grab");
                        else
                            playerAnimator.Play("Throw");
                    }
                    else {
                        playerAnimator.Play(abilities[i].animationName);
                        abilities[i].remainingCooldown = abilities[i].cooldown;
                    }
                }
            }
        }
    }

    private void AssignAbilityButtons() {
        for (int i = 0; i < abilities.Length; i++) {
            switch (abilities[i].abilityName) {
                case "Normal Attack": abilities[i].activationBtn = PlayerControls.Instance.controls.normalAttack; break;
                case "Ground Slam": abilities[i].activationBtn = PlayerControls.Instance.controls.groundSlam; break;
                case "Hammer Quake": abilities[i].activationBtn = PlayerControls.Instance.controls.hammerQuake; break;
                case "Fireball": abilities[i].activationBtn = PlayerControls.Instance.controls.fireball; break;
                case "Grab Throw": abilities[i].activationBtn = PlayerControls.Instance.controls.grabThrow; break;
            }
        }
    }
}
