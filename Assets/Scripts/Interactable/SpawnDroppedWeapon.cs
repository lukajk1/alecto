using UnityEngine;

public class SpawnDroppedWeapon : MonoBehaviour
{
    [SerializeField] private GameObject daggerDrop;
    [SerializeField] private GameObject sniperDrop;
    [SerializeField] private GameObject siphonDrop;
    [SerializeField] private GameObject barehandDrop;
    private void OnEnable()
    {
        CombatEventBus.OnWeaponDropped += CreateDrop;
    }
    private void OnDisable()
    {
        CombatEventBus.OnWeaponDropped -= CreateDrop;
    }

    public void CreateDrop(Weapon droppedWeapon)
    {
        //// let's just check if it's actually this that's causing the freeze
        ////Debug.Log(droppedWeapon is Barehand);
        ////Debug.Log(droppedWeapon);
        //if (droppedWeapon is Barehand || droppedWeapon == null) return;

        //GameObject drop = DecodeWeapon(droppedWeapon);

        //GameObject weaponDrop = Instantiate(drop, Game.i.PlayerTransform.position + new Vector3(0, 1.5f, 0) + Game.i.PlayerCamera.transform.forward * 1.5f, Quaternion.identity);
        //weaponDrop.GetComponentInChildren<WeaponPickup>().SetWeapon(droppedWeapon);

        ////Debug.Log($"instantiated at {Game.I.PlayerTransform.position + new Vector3(0, 2f, 0)}");
        ////Debug.Log($"player pos: {Game.I.PlayerTransform.position}");
    }    
    
    public void CreateDrop(Weapon droppedWeapon, Vector3 pos)
    {
        //// let's just check if it's actually this that's causing the freeze
        //if (droppedWeapon is Barehand || droppedWeapon == null) return;

        //GameObject drop = DecodeWeapon(droppedWeapon);

        //GameObject weaponDrop = Instantiate(drop, pos + new Vector3(0, 1.5f, 0), Quaternion.identity);
        //weaponDrop.GetComponentInChildren<WeaponPickup>().SetWeapon(droppedWeapon);
    }

    GameObject DecodeWeapon(Weapon weapon)
    {
        switch (weapon)
        {
            case BloodSiphon:
                return siphonDrop;
            case ChainDaggers:
                return daggerDrop;
            case Spear:
                return sniperDrop;
            default:
                return barehandDrop;
        }
    }
 }
