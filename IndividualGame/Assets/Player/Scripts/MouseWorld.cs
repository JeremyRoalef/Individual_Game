using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWorld : MonoBehaviour
{
    //Static reference to the mouse position
    public static MouseWorld instance;

    [SerializeField]
    LayerMask collisionInteractionLayers;

    [SerializeField]
    LayerMask groundLayers;

    private void Awake()
    {
        //Set singleton
        instance = this;
    }

    private void LateUpdate()
    {
        transform.position = GetMouseWorldPosition();
    }

    public static bool IsValidMousePositionOnGround()
    {
        bool _;
        RaycastHit raycastHit = GetMouseRaycastHit(out _);
        return IsValidMousePositionOnGround(raycastHit);
    }

    static bool IsValidMousePositionOnGround(RaycastHit raycastHit)
    {
        if (raycastHit.collider == null)
        {
            return false;
        }

        //Check the layer index of the object's layer & see if it is a ground layer
        if ((1 << raycastHit.transform.gameObject.layer & instance.groundLayers) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Vector3 GetMousePositionOnGround()
    {
        bool _ = false;
        RaycastHit raycastHit = GetMouseRaycastHit(out _);

        if (IsValidMousePositionOnGround(raycastHit))
        {
            return raycastHit.point;
        }

        //This should never run
        Debug.LogWarning("Warning: GetMousePositionOnGround() was called, but position was not valid!" +
            "Try testing if position on ground is valid using MouseWorld.IsValidMousePositionOnGround " +
            "before calling this method");

        return Vector3.positiveInfinity;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        bool hasHit = false;
        RaycastHit raycastHit = GetMouseRaycastHit(out hasHit);
        if (!hasHit)
        {
            return Vector3.zero;
        }

        return raycastHit.point;
    }

    public static GameObject GetHitGameObject()
    {
        bool hasHit = false;
        RaycastHit raycastHit = GetMouseRaycastHit(out hasHit);

        if (!hasHit) { return null; }
        return raycastHit.collider.gameObject;
    }

    private static RaycastHit GetMouseRaycastHit(out bool hasHit)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        //Debug.Log("mouse screen pos: " + mouseScreenPos);

        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        hasHit = Physics.Raycast(
            ray,
            out RaycastHit raycastHit,
            Mathf.Infinity,
            instance.collisionInteractionLayers,
            QueryTriggerInteraction.Ignore
            );

        return raycastHit;
    }
}
