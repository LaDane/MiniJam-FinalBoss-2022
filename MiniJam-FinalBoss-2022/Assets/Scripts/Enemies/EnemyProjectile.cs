using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [SerializeField] float durationMultiplier = 0.2f;

    private float timeElapsed;
    private float lerpDuration;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start() {
        startPos = transform.position;
        endPos = PlayerAbilityManager.Instance.playerCenterPoint.position;
        lerpDuration = Vector3.Distance(startPos, endPos) * durationMultiplier;
    }

    private void Update() {
        if (timeElapsed < lerpDuration) {
            transform.position = Vector3.Lerp(startPos, PlayerAbilityManager.Instance.playerCenterPoint.position, timeElapsed / lerpDuration);
            Vector3 direction = (PlayerAbilityManager.Instance.playerCenterPoint.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            timeElapsed += Time.deltaTime;
        }
        else {
            Destroy(gameObject);
        }
    }
}
