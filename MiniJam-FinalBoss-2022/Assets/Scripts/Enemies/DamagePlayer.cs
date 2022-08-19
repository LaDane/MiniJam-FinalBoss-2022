using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

    private ClassType classType;

    private void Start() {
        classType = transform.GetComponentInParent<AIUnit>().classType;
    }

    public void DoDamage() {
        PlayerHealthManager.Instance.DamagePlayer(classType);
    }
}
