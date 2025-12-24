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
        Move();
    }

    private void Move()
    {
        //Note: Movement is done via up/down/left/right, corresponding to +z/-z/+x/-x, respectively
        if (moveDir.sqrMagnitude <= float.Epsilon)
        {
            playerRb.linearVelocity = new Vector3(
                0,
                playerRb.linearVelocity.y,
                0
            );
            return;
        }

        Vector3 currentFlatVelocity = new Vector3(
            playerRb.linearVelocity.x,
            0,
            playerRb.linearVelocity.z
            );

        Vector3 playerVelocity = new Vector3(
            moveDir.x * moveSpeed,
            0,
            moveDir.y * moveSpeed
        );

        Vector3 changeInVelocity = playerVelocity - currentFlatVelocity;

        playerRb.AddForce(changeInVelocity, ForceMode.VelocityChange);
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        Vector3 lookDir = playerRb.linearVelocity.normalized;
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
