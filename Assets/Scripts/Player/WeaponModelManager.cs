using System.Collections.Generic;
using UnityEngine;

public class WeaponModelManager : MonoBehaviour
{
    [SerializeField] private GameObject dagger;
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject barehand;
    [SerializeField] private GameObject siphon;
    private List<GameObject> weapons;
    private Weapon currentWeapon; void Awake()
    {
        weapons = new List<GameObject> { dagger, spear, barehand, siphon };
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }
    }
    private void OnEnable()
    {
        CombatEventBus.OnInventoryUpdated += SwitchModel;
    }
    private void OnDisable()
    {
        CombatEventBus.OnInventoryUpdated -= SwitchModel;
    }
    void SwitchModel()
    {
        currentWeapon = Inventory.I.GetActiveWeapon();
        GameObject model;

        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        if (currentWeapon is BloodSiphon) model = siphon;
        else if (currentWeapon is Barehand) model = barehand;
        else if (currentWeapon is Spear) model = spear;
        else if (currentWeapon is ChainDaggers) model = dagger;
        else model = barehand; // just here to prevent use of unassigned field

        model.SetActive(true);
        model.GetComponent<WeaponAnimator>().SwitchIn();
    }
}
