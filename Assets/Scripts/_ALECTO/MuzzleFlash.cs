using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject muzzleFlash;

    private void OnEnable()
    {
        CombatEventBus.OnWeaponFired += Fire;
    }

    private void OnDisable()
    {

        CombatEventBus.OnWeaponFired -= Fire;
    }

    void Fire(Weapon weapon)
    {
        Instantiate(muzzleFlash, transform.position, Quaternion.identity, transform);
    }
}
