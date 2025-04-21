using System;
using UnityEngine;

public static class CombatEventBus
{
    public static event Action<Weapon> OnWeaponDropped;
    public static void BCOnWeaponDropped(Weapon droppedWeapon) => OnWeaponDropped?.Invoke(droppedWeapon);

    public static event Action OnInventoryUpdated;

    public static void BCOnInventoryUpdated() => OnInventoryUpdated?.Invoke();

    public static event Action<Weapon> OnWeaponFired;

    public static void BCOnWeaponFired(Weapon weapon) => OnWeaponFired?.Invoke(weapon);

    public static event Action OnAmmoCountsModified;
    public static void BCOnAmmoCountsModified() => OnAmmoCountsModified?.Invoke();

    public static event Action<int, bool, Vector3> OnEnemyHit;

    public static void BCOnEnemyHit(int damage, bool isCrit, Vector3 pos) => OnEnemyHit?.Invoke(damage, isCrit, pos);

    public static event Action<int, bool> OnPlayerHit;
    public static void BCOnPlayerHit(int damage, bool isCrit) => OnPlayerHit?.Invoke(damage, isCrit);

    public static event Action<EnemyUnit, Vector3> OnEnemyDeath;
    public static void BCOnEnemyDeath(EnemyUnit enemyType, Vector3 pos) => OnEnemyDeath?.Invoke(enemyType, pos);

    public static event Action OnWeaponBreak;
    public static void BCOnWeaponBreak() => OnWeaponBreak?.Invoke();   

    public static event Action OnPlayerHeal;
    public static void BCOnPlayerHeal() => OnPlayerHeal?.Invoke();   

}
