using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWoW : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 0.1f;
    [SerializeField] private float gravity = 3f;

    private Vector2 inputs;
    private float rotation;
    private Vector2 mouseRotation;
    private Vector3 velocity;

    private Vector3 cameraStartPosition = new Vector3(0, 21f, -6.5f);
    private Vector3 cameraStartRotation = new Vector3(50, 0, 0);

    void Start() {
        cam.transform.localPosition = cameraStartPosition;
        cam.transform.localRotation = Quaternion.Euler(cameraStartRotation);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (!PlayerHealthManager.Instance.isAlive) {
            inputs = Vector2.zero;
            return;
        }
        if(Time.timeScale == 0) {
            return;
        }
        GetInputs();
        Locomotion();
        AnimatePlayer();

        // Fix player pos
        animator.transform.localPosition = new Vector3(0, -0.2f, 0);
    }

    private void AnimatePlayer() {
        animator.SetFloat("Velocity Z", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Velocity X", Input.GetAxis("Horizontal"));

        //animator.SetFloat("Velocity Z", inputs.y);
        //animator.SetFloat("Velocity X", inputs.x);
    }

    void Locomotion() {
        // Keyboard rotation
        Vector3 orientation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed * Time.deltaTime, 0);
        transform.eulerAngles = orientation;

        // Mouse rotation
        transform.Rotate(0, mouseRotation.x, 0);
        if (cam.transform.eulerAngles.x + (-mouseRotation.y) > 80 && cam.transform.eulerAngles.x + (-mouseRotation.y) < 340) { }
        else {
            cam.transform.RotateAround(transform.position, cam.transform.right, -mouseRotation.y);
        }

        // Movement
        velocity = (transform.forward * inputs.normalized.y + transform.right * inputs.normalized.x) * moveSpeed * Time.deltaTime;
        velocity.y = -gravity * Time.deltaTime;
        controller.Move(velocity);
        //transform.position += velocity;
    }

    void GetInputs() {

        // Forwards & backwards
        if (Input.GetKey(PlayerControls.Instance.controls.forwards)) {
            inputs.y = 1;
        }
        if (Input.GetKey(PlayerControls.Instance.controls.backwards)) {
            if (Input.GetKey(PlayerControls.Instance.controls.forwards)) {
                inputs.y = 0;
            }
            else {
                inputs.y = -1;
            }
        }
        if (!Input.GetKey(PlayerControls.Instance.controls.forwards) && !Input.GetKey(PlayerControls.Instance.controls.backwards)) {
            inputs.y = 0;
        }

        // Strafe left & right
        if (Input.GetKey(PlayerControls.Instance.controls.strafeLeft)) {
            inputs.x = -1;
        }
        if (Input.GetKey(PlayerControls.Instance.controls.strafeRight)) {
            if (Input.GetKey(PlayerControls.Instance.controls.strafeLeft)) {
                inputs.x = 0;
            }
            else {
                inputs.x = 1;
            }
        }
        if (!Input.GetKey(PlayerControls.Instance.controls.strafeLeft) && !Input.GetKey(PlayerControls.Instance.controls.strafeRight)) {
            inputs.x = 0;
        }

        // Rotate left & right
        if (Input.GetKey(PlayerControls.Instance.controls.rotateLeft)) {
            rotation = -1;
        }
        if (Input.GetKey(PlayerControls.Instance.controls.rotateRight)) {
            if (Input.GetKey(PlayerControls.Instance.controls.rotateLeft)) {
                rotation = 0;
            }
            else {
                rotation = 1;
            }
        }
        if (!Input.GetKey(PlayerControls.Instance.controls.rotateLeft) && !Input.GetKey(PlayerControls.Instance.controls.rotateRight)) {
            rotation = 0;
        }

        // Mouse rotation
        mouseRotation.x = Input.GetAxis("Mouse X") * PlayerControls.Instance.controls.mouseSensitivity;
        mouseRotation.y = Input.GetAxis("Mouse Y") * PlayerControls.Instance.controls.mouseSensitivity;
        
        //if (Input.GetMouseButton(1)) {
        //    mouseRotation.x = Input.GetAxis("Mouse X") * PlayerControls.Instance.controls.mouseSensitivity;
        //    mouseRotation.y = Input.GetAxis("Mouse Y") * PlayerControls.Instance.controls.mouseSensitivity;
        //}
        //else {
        //    mouseRotation = Vector2.zero;
        //}
    }
}
