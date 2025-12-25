using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    [SerializeField]
    PlayerInput playerInput;

    [SerializeField]
    bool toggleMove;
    bool canMove = true;

    [SerializeField]
    bool toggleCombat;
    bool canCombat = false;

    static readonly Dictionary<ActionMap, string> ACTION_MAPS = new Dictionary<ActionMap, string>
    {
        { ActionMap.World, "World" },
        { ActionMap.UI, "UI" },
        { ActionMap.Combat, "Combat" },
        { ActionMap.Build, "Build" },
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Warning: Multiple game states found in scene. Ensure only one game state exists in the scene.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (toggleMove)
        {
            canMove = !canMove;
            ToggleActionMap(ActionMap.World, canMove);
            toggleMove = false;
        }
        if (toggleCombat)
        {
            canCombat = !canCombat;
            Debug.Log("Combat enabled: " + canCombat);
            ToggleActionMap(ActionMap.Combat, canCombat);
            toggleCombat = false;
        }
    }

    public void ToggleActionMap(ActionMap actionMap, bool enable)
    {
        if (!ACTION_MAPS.TryGetValue(actionMap, out string actionMapName))
        {
            Debug.LogWarning($"Warning: {actionMap} not found in action map dictionary.");
            return;
        }

        InputActionMap map = playerInput.actions.FindActionMap(actionMapName, true);

        if (enable)
            map.Enable();
        else
            map.Disable();
    }
}
