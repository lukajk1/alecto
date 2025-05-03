using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private int weaponId; // only used for generating a new weapon
    public Gun weapon;
    private bool generateNewWeapon = true;
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetWeapon(Gun weapon)
    {
        generateNewWeapon = false;
        this.weapon = weapon;
    }

    public override void OnInteract()
    {
        if (generateNewWeapon)
        {
            switch (weaponId)
            {
                case 0:
                    weapon = new Humboldt();
                    break;
                case 1:
                    weapon = new BloodSiphon();
                    break;
                case 2:
                    weapon = new DeagleShotgun();
                    break;
                case 3:
                    weapon = new Spear();
                    break;
                default:
                    weapon = new Humboldt();
                    break;
            }
        }

        Inventory.I.AddWeapon(weapon);
        Destroy(transform.root.gameObject);
    }
}
