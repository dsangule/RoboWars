using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;
    [Space]
    public float walkSpeed = 8f;
    public float sprintSpeed = 12f;
    public float maxVelocityChange = 10f;
    [Space]
    public float airControl = 0.5f;
    [Space]
    public float jumpHeight = 7f;

    private Vector2 input;
    private Rigidbody rb;

    private bool isSprinting;
    private bool isJumping;
    private bool isGrounded = false;
    private bool isPaused;

    private float baseFov;
    private float sprintFovModifier = 1.25f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        baseFov = playerCam.fieldOfView;
    }
    private void OnTriggerStay(Collider other) {
        isGrounded = true;
    }

    void Update() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        isSprinting = Input.GetButton("Sprint") && input.y > 0.5f;
        isJumping = Input.GetButton("Jump");
        isPaused = Input.GetKeyDown(KeyCode.Escape);

        if (isPaused) {
            GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
        }
        if (Pause.paused) {
            isSprinting = false;
            isJumping = false;
            isGrounded = false;
        }
    }

    void FixedUpdate() {
        if (Pause.paused) {
            input = Vector2.zero;
            isSprinting = false;
            isJumping = false;
            isGrounded = false;
        }
        if (isGrounded) {
            if (isJumping) {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            } else if (input.magnitude > 0.5f) {
                rb.AddForce(CalculateMovement(isSprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            } else {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        } else {
            if (input.magnitude > 0.5f) {
                rb.AddForce(CalculateMovement(isSprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            } else {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        if (isSprinting) {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, baseFov * sprintFovModifier, Time.deltaTime * 8f);
        } else {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, baseFov, Time.deltaTime * 8f);
        }
        isGrounded = false;
    }

    Vector3 CalculateMovement(float _speed) {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f) {
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            return (velocityChange);
        } else {
            return new Vector3();
        }
    }
}
