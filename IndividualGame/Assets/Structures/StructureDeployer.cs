using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StructureDeployer : MonoBehaviour
{
    [SerializeField]
    InputActionReference buildAction;

    //TODO: Update the structure selection to allow the player to select a structure via UI
    [SerializeField]
    StructureDeploymentTester currentStructurePrefab;

    StructureDeploymentTester currentStructureObj;

    private void OnEnable()
    {
        buildAction.action.performed += HandleBuildPerformed;
    }

    private void OnDisable()
    {
        buildAction.action.performed -= HandleBuildPerformed;
    }

    private void Start()
    {
        if (currentStructurePrefab == null) return;
        currentStructureObj = Instantiate(currentStructurePrefab);
    }

    private void HandleBuildPerformed(InputAction.CallbackContext context)
    {
        if (!currentStructureObj.CanBePlaced()) return;
        currentStructureObj.PlaceStructure();
    }
}
