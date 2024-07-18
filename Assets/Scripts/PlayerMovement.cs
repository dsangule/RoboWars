using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;
    [Space]
    public float airControl = 0.5f;
    [Space]
    public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;

    private bool isSprinting;
    private bool isJumping;
    private bool isGrounded = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        isSprinting = Input.GetButton("Sprint");
        isJumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other) {
        isGrounded = true;
    }

    void FixedUpdate() {
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
