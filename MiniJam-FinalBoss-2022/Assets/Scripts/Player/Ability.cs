using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability {

    public string abilityName;
    public string animationName;
    public float thrust;
    //public float sizeMultiplier;
    public float cooldown;
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public KeyCode activationBtn;
}
