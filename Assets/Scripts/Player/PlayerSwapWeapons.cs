using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerSwapWeapons : MonoBehaviour
{

    private Inventory inventory;
    private float scrollSwapCD = 0.3f;
    private float scrollSwapCDCount = 0;

    private void Start()
    {
        inventory = Inventory.I;
    }
    private void Update()
    {
        scrollSwapCDCount -= Time.deltaTime;

        Vector2 scroll = Mouse.current.scroll.ReadValue();
        float scrollY = scroll.y;

        if (scrollY != 0 && scrollSwapCDCount <= 0f || Input.GetKeyDown(KeyCode.Q))
        {
            inventory.SwapActiveWeapon();
            scrollSwapCDCount = scrollSwapCD;
        }


        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            inventory.SetActiveSlot(0);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            inventory.SetActiveSlot(1);
        }
    }
}
