using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private Camera fpCamera;
    [SerializeField] private PlayerInput playerInput;
    private Weapon weapon;
    private InputAction attackAction;
    private void OnEnable()
    {
        MainEventBus.OnRunStart += Setup;
        CombatEventBus.OnInventoryUpdated += UpdateActiveWeapon;
        playerInput.actions["Reload"].performed += OnReloadPressed;
        attackAction = playerInput.actions["Attack"];
        attackAction.Enable();
    }
    private void OnDisable()
    {
        MainEventBus.OnRunStart -= Setup;
        CombatEventBus.OnInventoryUpdated -= UpdateActiveWeapon;
        playerInput.actions["Reload"].performed -= OnReloadPressed;
        attackAction.Disable();
    }
    public void Setup()
    {
        UpdateActiveWeapon();
    }
    private void Update()
    {
        if (attackAction.IsPressed() && weapon != null)
        {
            weapon.Fire(fpCamera.transform.position + (fpCamera.transform.forward * 0.3f), fpCamera.transform.forward); // small offset to move out of head hitbox or whatever player hitbox is blocking it
        }
    }

    private void UpdateActiveWeapon()
    {
        weapon = Inventory.I.GetActiveWeapon();
    }

    private void OnReloadPressed(InputAction.CallbackContext ctx)
    {
        weapon.Reload();
    }

}
