using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlOptions {

    [Header("Movement")]
    //public KeyCode forwards, backwards, strafeLeft, strafeRight, rotateLeft, rotateRight, Attack, GroundSlam, HammerQuake, Fireball, GrabThrow;
    public KeyCode forwards;
    public KeyCode backwards;
    public KeyCode strafeLeft;
    public KeyCode strafeRight;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;

    [Header("Camera")]
    public float mouseSensitivity;

    [Header("Abilities")]
    public KeyCode normalAttack;
    public KeyCode groundSlam;
    public KeyCode hammerQuake;
    public KeyCode fireball;
    public KeyCode grabThrow;
}
