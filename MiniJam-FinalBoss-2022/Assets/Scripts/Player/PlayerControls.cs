using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private static PlayerControls _instance;
    public static PlayerControls Instance {
        get {
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    public ControlOptions controls;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
}
