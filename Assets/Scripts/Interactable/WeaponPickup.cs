using UnityEngine;

public class WeaponPickup : Interactable
{
    [SerializeField] private int weaponId; // only used for generating a new weapon
    public Weapon weapon;
    private bool generateNewWeapon = true;
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetWeapon(Weapon weapon)
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
                    weapon = new Barehand();
                    break;
                case 1:
                    weapon = new BloodSiphon();
                    break;
                case 2:
                    weapon = new ChainDaggers();
                    break;
                case 3:
                    weapon = new Spear();
                    break;
                default:
                    weapon = new Barehand();
                    break;
            }
        }

        Inventory.I.AddWeapon(weapon);
        Destroy(transform.root.gameObject);
    }
}
