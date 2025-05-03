using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory I;
    private Gun[] inv = new Gun[2];
    public int ActiveSlot {  get; private set; }
    private void Awake()
    {
        if (I != null) Debug.LogError("multiple singletons in scene");
        I = this;

        ActiveSlot = 0;

        inv[0] = new Humboldt();
        inv[1] = new DeagleShotgun();
    }

    private void OnEnable()
    {
        CombatEventBus.OnWeaponBreak += OnWeaponBreak;    
    }
    
    private void OnDisable()
    {
        CombatEventBus.OnWeaponBreak -= OnWeaponBreak;    
    }


    private void Start()
    {
        CombatEventBus.BCOnInventoryUpdated(); // just to push an update so all relevant systems know what's the starting weapon
    }
    public void SetActiveSlot(int slot)
    {
        if (slot != ActiveSlot && (slot == 0 || slot == 1))
        {
            ActiveSlot = slot;
            CombatEventBus.BCOnInventoryUpdated();
        }
    }
    public void SetWeapon(int slot, Gun weapon)
    {
        if (!(inv[slot] is Humboldt))
        {
            CombatEventBus.BCOnWeaponDropped(inv[slot]);
        }

        inv[slot] = weapon; // equip new weapon
        ActiveSlot = slot;
        CombatEventBus.BCOnInventoryUpdated();

    }
    private void OnWeaponBreak()
    {
        if (inv[0].IsBroken)
        {
            if (inv[1] != null)
            {
                inv[0] = inv[1];
                inv[1] = null;
            }
            ActiveSlot = 0;
        }
        else if (inv[1].IsBroken)
        {
            inv[1] = null;
            ActiveSlot = 0;
        }
        CombatEventBus.BCOnInventoryUpdated();
    }

    public void AddWeapon(Gun weapon) // for equipping picked up weapons
    {
        if (inv[0] is Humboldt)
        {
            if (inv[1] == null)
            {
                SetWeapon(1, weapon);
            }
            else
            {
                SetWeapon(0, weapon);
            }
        }
        else
        {
            SetWeapon(ActiveSlot, weapon);
        }
        CombatEventBus.BCOnInventoryUpdated();
    }
    public void SwapActiveWeapon()
    {
        if (inv[1 -  ActiveSlot] != null) SetActiveSlot(1 - ActiveSlot);
    }

    public Gun GetWeapon(int slot) 
    { 
        return inv[slot];
    }

    public Gun GetActiveWeapon()
    {
        return inv[ActiveSlot];
    }

    public int GetActiveSlot()
    {
        return ActiveSlot;
    }
}
