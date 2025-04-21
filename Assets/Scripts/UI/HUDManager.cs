using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slot0Gun;
    [SerializeField] private TextMeshProUGUI slot1Gun;
    [SerializeField] private TextMeshProUGUI slot0Number;
    [SerializeField] private TextMeshProUGUI slot1Number;
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private TextMeshProUGUI totalAmmo;
    [SerializeField] private TextMeshProUGUI txtHealth;
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider wallClimbStamina;
    private Weapon activeWeapon;

    private void OnEnable()
    {
        CombatEventBus.OnInventoryUpdated += OnInventoryUpdated;
        CombatEventBus.OnAmmoCountsModified += UpdateAmmoHUD;
        CombatEventBus.OnPlayerHit += OnPlayerHit;
        CombatEventBus.OnPlayerHeal += OnPlayerHeal;
        MainEventBus.OnRunStart += Setup;
    }

    private void OnDisable()
    {
        CombatEventBus.OnInventoryUpdated -= OnInventoryUpdated;
        CombatEventBus.OnAmmoCountsModified -= UpdateAmmoHUD;
        CombatEventBus.OnPlayerHit -= OnPlayerHit;
        CombatEventBus.OnPlayerHeal -= OnPlayerHeal;
        MainEventBus.OnRunStart -= Setup;
    }
    private void Setup()
    {
        UpdateHealth();
        OnInventoryUpdated();
    }
    private void OnPlayerHit(int damage, bool isCrit)
    {
        UpdateHealth();
    }
    private void OnPlayerHeal()
    {
        UpdateHealth();
    }
    private void UpdateHealth()
    {
        txtHealth.text = $"{Game.i.PlayerUnitInstance.CurrentHealth} / {Game.i.PlayerUnitInstance.CurrentMaxHealth}";
        healthbar.value = (float)Game.i.PlayerUnitInstance.CurrentHealth / Game.i.PlayerUnitInstance.CurrentMaxHealth;
    }
    private void OnInventoryUpdated()
    {
        activeWeapon = Inventory.I.GetActiveWeapon();

        slot0Gun.text = Inventory.I.GetWeapon(0).Name; // don't need to check if this is null because there will always be a base weapon equipped

        Weapon one = Inventory.I.GetWeapon(1);

        if (one != null)
        {
            slot1Gun.text = one.Name;
        }
        else
        {
            {
                slot1Gun.text = "";
            }
        }

        Color grayedOut = new Color(1, 1, 1, 0.2f);
        Color active = new Color(1, 1, 1, 0.6f);

        if (Inventory.I.GetActiveSlot() == 0)
        {
            slot0Gun.color = active;
            slot0Number.color = active;
            slot1Gun.color = grayedOut;
            slot1Number.color = grayedOut;
        }
        else
        {
            slot0Gun.color = grayedOut;
            slot0Number.color = grayedOut;
            slot1Gun.color = active;
            slot1Number.color = active;
        }

        UpdateAmmoHUD();
    }

    private void UpdateAmmoHUD()
    {
        //Debug.Log(activeWeapon);
        if (activeWeapon != null)
        {
            ammo.text = $"{activeWeapon.CurrentAmmo} / {activeWeapon.ClipSize}";
            totalAmmo.text = $"reserve: {activeWeapon.TotalAmmo}";
        }
    }

    public void SetWallClimbStamina(float value)
    {
        //wallClimbStamina.value = value;
    }
    public void SetWallClimbMeter(bool value)
    {
        //wallClimbStamina.gameObject.SetActive(value);
    }
}
