using System;
using Unity.VisualScripting;
using UnityEngine;

public class StructureDeploymentTester : MonoBehaviour
{
    [SerializeField, Tooltip("The actual structure object that will exist in the game")]
    Structure realStructurePrefab;

    [SerializeField]
    Material canPlaceMaterial;

    [SerializeField]
    Material cannotPlaceMaterial;

    [SerializeField]
    MeshRenderer[] renderers;

    private void Update()
    {
        bool mouseIsGrounded = MouseWorld.IsValidMousePositionOnGround();
        Debug.Log(mouseIsGrounded);
        
        if (!mouseIsGrounded)
        {
            HideStructure();
        }
        else
        {
            ShowTestStructure();
        }

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = CanBePlaced()? canPlaceMaterial: cannotPlaceMaterial;
        }
        
        transform.position = MouseWorld.instance.transform.position;
    }

    public bool CanBePlaced()
    {
        //TODO: Add criteria for deployment:
        /*
         * Is the ground flat? (min vs max height level reasonable? no slope too steep?)
         * Is the structure too close to others?
         * Is the position buildable?
         * Is pathfinding preserved (yes if set up correctly)
         * Are blocker objects blocking the path?
         */

        return false;
    }

    public void ShowTestStructure()
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    public void PlaceStructure()
    {
        Instantiate(realStructurePrefab, transform.position, Quaternion.identity);
    }

    public void HideStructure()
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
