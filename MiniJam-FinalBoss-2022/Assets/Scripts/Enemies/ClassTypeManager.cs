using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassType {
    Tank,
    Healer,
    Mage,
    Warlock,
    Rogue
}

public class ClassTypeManager : MonoBehaviour {

    [Header("Tank Ranges")]
    [SerializeField] private float tankMax = 6;
    [SerializeField] private float tankIdeal = 4;
    
    [Header("Healer Ranges")]
    [SerializeField] private float healerMax = 20;
    [SerializeField] private float healerIdeal = 15;
    
    [Header("Mage Ranges")]
    [SerializeField] private float mageMax = 20;
    [SerializeField] private float mageIdeal = 15;
    
    [Header("Warlock Ranges")]
    [SerializeField] private float warlockMax = 25;
    [SerializeField] private float warlockIdeal = 20;

    [Header("Rogue Ranges")]
    [SerializeField] private float rogueMax = 6;
    [SerializeField] private float rogueIdeal = 4;

    private static ClassTypeManager _instance;
    public static ClassTypeManager Instance {
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

    public Vector2 GetClassTypeRanges(ClassType classType) {
        switch (classType) {
            case ClassType.Tank: return new Vector2(tankMax, tankIdeal);
            case ClassType.Healer: return new Vector2(healerMax, healerIdeal);
            case ClassType.Mage: return new Vector2(mageMax, mageIdeal);
            case ClassType.Warlock: return new Vector2(warlockMax, warlockIdeal);
            case ClassType.Rogue: return new Vector2(rogueMax, rogueIdeal);
        }
        return Vector2.zero;
    }
}
