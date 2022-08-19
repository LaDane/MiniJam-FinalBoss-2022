using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability {

    [Header("Names")]
    public string abilityName;
    public string animationName;

    [Header("Cooldown")]
    public float cooldown;
    [HideInInspector] public float remainingCooldown;

    [Header("Projectile")]
    public GameObject projectile;
    public float lifeSpan;
    public float slowDownRate;
    public float speed;
    public float spawnDelay;
    public float destroyDelay;
    //public float sizeMultiplier;

    [Header("Ragdoll Collider")]
    public float thrust;
    public float size;

    [HideInInspector] public KeyCode activationBtn;
}
