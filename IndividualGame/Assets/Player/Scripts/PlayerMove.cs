using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    InputActionReference moveInput;

    [SerializeField]
    Rigidbody playerRb;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float rotationSpeed = 720f;

    Vector2 moveDir = new Vector2();
    Vector3 groundNormal = Vector3.up;
    bool isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("collsiion stay");

        foreach (var contact in collision.contacts)
        {
            // Consider ground-like surfaces only
            if (contact.normal.y > 0.1f)
            {
                groundNormal = contact.normal;
                isGrounded = true;
                continue;
            }
        }
    }

    private void OnEnable()
    {
        moveInput.action.performed += OnMoveInputPerformed;
        moveInput.action.canceled += OnMoveInputPerformed;
    }

    private void OnDisable()
    {
        moveInput.action.performed -= OnMoveInputPerformed;
        moveInput.action.canceled -= OnMoveInputPerformed;
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        groundNormal = Vector3.up;

        Move();
    }

    private void Move()
    {
        //Note: Movement is done via up/down/left/right, corresponding to +z/-z/+x/-x, respectively
        if (moveDir.sqrMagnitude <= float.Epsilon)
        {
            playerRb.linearVelocity = new Vector3(
                0,
                playerRb.linearVelocity.y > 0? 0: playerRb.linearVelocity.y,
                0
            );
            return;
        }

        Vector3 desiredMove = new Vector3(moveDir.x, 0f, moveDir.y);

        // Project onto slope
        if (isGrounded)
        {
            desiredMove = Vector3.ProjectOnPlane(desiredMove, groundNormal);
        }

        desiredMove.Normalize();

        Vector3 currentFlatVelocity = new Vector3(
            playerRb.linearVelocity.x,
            0,
            playerRb.linearVelocity.z
            );

        Vector3 playerVelocity = desiredMove * moveSpeed;

        Vector3 changeInVelocity = playerVelocity - currentFlatVelocity;
        if (isGrounded)
        {
            changeInVelocity = Vector3.ProjectOnPlane(changeInVelocity, groundNormal);
        }

        playerRb.AddForce(changeInVelocity, ForceMode.VelocityChange);
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        Vector3 lookDir = new Vector3(
            playerRb.linearVelocity.x,
            0,
            playerRb.linearVelocity.z
            );
        lookDir.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        Quaternion newRotation = Quaternion.RotateTowards(
            playerRb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
            );
        playerRb.MoveRotation(newRotation);
    }

    private void OnMoveInputPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Move input performed");
        moveDir = context.ReadValue<Vector2>();
    }
}
