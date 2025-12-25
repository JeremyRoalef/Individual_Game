using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField]
    InputActionReference actionReference;

    Vector2 moveDir;

    private void Update()
    {
        Debug.Log(moveDir);
    }

    private void OnEnable()
    {
        actionReference.action.performed += OnMove;
        actionReference.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        actionReference.action.performed -= OnMove;
        actionReference.action.canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
}
