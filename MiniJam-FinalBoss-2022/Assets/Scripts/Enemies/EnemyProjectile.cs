using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [SerializeField] float durationMultiplier = 0.2f;
    
    private float timeElapsed;
    private float lerpDuration;
    private Vector3 startPos;
    public Transform endPos;
    private bool waitingDestroy = false;

    private void Start() {
        startPos = transform.position;
        endPos = PlayerAbilityManager.Instance.playerCenterPoint;
        lerpDuration = Vector3.Distance(startPos, endPos.position) * durationMultiplier;
    }

    private void Update() {
        if (timeElapsed < lerpDuration) {
            transform.position = Vector3.Lerp(startPos, endPos.position, timeElapsed / lerpDuration);
            Vector3 direction = (PlayerAbilityManager.Instance.playerCenterPoint.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            timeElapsed += Time.deltaTime;
        }
        else {
            if (!waitingDestroy) {
                Destroy(gameObject, 1f);
            }
        }
    }
}
