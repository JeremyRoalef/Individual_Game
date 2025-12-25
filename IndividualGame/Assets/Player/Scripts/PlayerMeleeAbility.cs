using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAbility : MonoBehaviour
{
    public event Action OnPlayerMelee;

    [SerializeField]
    InputActionReference meleeAction;

    [SerializeField]
    float meleeRange;

    [SerializeField]
    float delayBetweenMelees;

    [SerializeField]
    float damageDelay;

    bool canMelee = true;

    private void OnEnable()
    {
        Debug.Log("hi");
        meleeAction.action.performed += HandleMeleeInput;
    }

    private void OnDisable()
    {
        meleeAction.action.performed -= HandleMeleeInput;
    }

    private void HandleMeleeInput(InputAction.CallbackContext context)
    {
        GameObject hitTarget = MouseWorld.GetHitGameObject();
        if (!CanHitTarget(hitTarget)) return;
        MeleeTarget(hitTarget);
    }

    private bool CanHitTarget(GameObject hitTarget)
    {
        if (!hitTarget)
        {
            //Debug.Log("Hit nothing");
            return false;
        }
        if (Vector3.Distance(transform.position, hitTarget.transform.position) > meleeRange)
        {
            //Debug.Log("Target too far");
            return false;
        }
        if (!canMelee)
        {
            //Debug.Log("cannot melee");
            return false;
        }

        return true;
    }

    private void MeleeTarget(GameObject hitTarget)
    {
        Debug.Log("Player meleed target: " + hitTarget.name);
        OnPlayerMelee?.Invoke();
        canMelee = false;
        StartCoroutine(ResetMelee());
        StartCoroutine(PerformMeleeDamage());
    }

    IEnumerator ResetMelee()
    {
        yield return new WaitForSeconds(delayBetweenMelees);
        canMelee = true;
    }

    IEnumerator PerformMeleeDamage()
    {
        yield return new WaitForSeconds(damageDelay);
        //TODO: Perform damage
        Debug.Log("Damaged target");
    }
}
