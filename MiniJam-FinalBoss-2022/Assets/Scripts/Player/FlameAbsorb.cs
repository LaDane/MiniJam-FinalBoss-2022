using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAbsorb : MonoBehaviour {

    [SerializeField] private float absorbDuration = 2f;
    [SerializeField] private Transform playerCenterPoint;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Absorbables")) {
            other.enabled = false;
            StartCoroutine(MoveFlame(other.transform));
        }
    }

    private IEnumerator MoveFlame(Transform flame) {
        float timeElapsed = 0;
        Vector3 startPos = flame.position;
        while (timeElapsed < absorbDuration) {
            flame.position = Vector3.Lerp(startPos, playerCenterPoint.position, timeElapsed / absorbDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        PlayerHealthManager.Instance.playerHP += PlayerHealthManager.Instance.regenAmount;
        if (PlayerHealthManager.Instance.playerHP > PlayerHealthManager.Instance.playerMaxHP) {
            PlayerHealthManager.Instance.playerHP = PlayerHealthManager.Instance.playerMaxHP;
        }
        Destroy(flame.gameObject, 0.5f);
    }
}
