using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTopDown : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float gravity = 3f;
    [SerializeField] LayerMask groundLayer;

    [Header("Camera Position")]
    [SerializeField] private float camDistanceFromPlayer = 50f;
    [SerializeField] private float camUpDownAxisOffset = -15f;
    [SerializeField] private float camRotation = 70;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Vector3 cameraStartPosition;
    private Vector3 cameraStartRotation;

    private void Start() {
        cam.transform.parent = null;

        cameraStartPosition = new Vector3(0, camDistanceFromPlayer, camUpDownAxisOffset);
        cameraStartRotation = new Vector3(camRotation, 0, 0);

        cam.transform.localPosition = cameraStartPosition;
        cam.transform.localRotation = Quaternion.Euler(cameraStartRotation);
    }

    private void Update() {
        if (!PlayerHealthManager.Instance.isAlive) {
            return;
        }
        HandleMovement();
    }

    private void FixedUpdate() {
        if (!PlayerHealthManager.Instance.isAlive) {
            return;
        }
        HandleRotation();
    }

    private void HandleMovement() {
        // Player movement
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveInput = moveInput.normalized;
        moveVelocity = moveInput * moveSpeed;
        moveVelocity.y = -gravity;
        controller.Move(moveVelocity * Time.deltaTime);

        // Camera movement
        cam.transform.localPosition = transform.position + cameraStartPosition;
    }

    private void HandleRotation() {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);

        // Cast raycast to a layer
        //RaycastHit hit;
        //if (Physics.Raycast(cameraRay, out hit, 99999, groundLayer)) {
        //    transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        //}

        // Cast raycast to a plane
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 hitPoint = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(hitPoint.x, transform.position.y, hitPoint.z));
        }
    }
}
