using UnityEngine;

public abstract class Weapon
{
    private float criticalHitModifier = 3.0f;
    public abstract string Name { get; }
    public abstract int ClipSize { get; }
    protected int totalAmmo;
    public int TotalAmmo 
    { 
        get => totalAmmo;
        set
        {
            if (value >= 0)
            {
                totalAmmo = value;
                CombatEventBus.BCOnAmmoCountsModified();
            }
        }
    }

    protected int _currentAmmo;
    public int CurrentAmmo
    {
        get => _currentAmmo;
        set
        {
            if (value >= 0 && value <= ClipSize)
            {
                _currentAmmo = value;
                CombatEventBus.BCOnAmmoCountsModified();
            }
        }
    }

    public abstract int BaseDamage { get; }
    public abstract float FireRate { get; }
    public abstract float ReloadSpeed { get; }
    public abstract float ReadySpeed { get; } // how fast a gun can be fired after being switched to
    public abstract float LifestealRatio { get; }
    public abstract float Range { get; }
    public bool IsBroken { get; protected set; }

    public float[,] DamageFalloffTable;
    protected WeaponTimer weaponTimer;
    protected RaycastHit hit;

    public Weapon()
    {
        CurrentAmmo = ClipSize;
        TotalAmmo = DecideInitialTotalAmmo();
        weaponTimer = new GameObject($"Generic {Name} Timer").AddComponent<WeaponTimer>();
        weaponTimer.Setup(this);
    }
    public abstract void Fire(Vector3 firingOrigin, Vector3 forwardFacingVector);

    protected virtual int DecideInitialTotalAmmo()
    {
        //return Random.Range(8, 16) * ClipSize;
        return 99999;
    }

    protected virtual bool TryFire()
    {
        if (CurrentAmmo <= 0)
        {
            return false;
        }
        if (weaponTimer.QueryCanFire())
        {
            SFXManager.i.PlaySFXClip(UISFXList.i.weaponFire, Game.i.PlayerTransform.position); // eventually this will be weapon specific and will be moved out of here
            CurrentAmmo--;
            CombatEventBus.BCOnWeaponFired(this);

            if (_currentAmmo <= 0)
            {
                if (!weaponTimer.IsReloading())
                {
                    Reload();
                    SFXManager.i.PlaySFXClip(UISFXList.i.outOfBullets, Game.i.PlayerTransform.position);
                }
            }

            return true;
        }
        else return false;
    }
    public virtual void Reload()
    {
        if (TotalAmmo >= ClipSize)
        {
            weaponTimer.Reload(ClipSize - CurrentAmmo);
        }
        else if (TotalAmmo > 0 && TotalAmmo < ClipSize)
        {
            weaponTimer.Reload(TotalAmmo);
            TotalAmmo = 0;
        }
        else if (TotalAmmo == 0 && CurrentAmmo == 0) 
        {
            Break(); // weapon is destroyed when it runs out of ammo
        }
    }
    protected virtual void Break() 
    {
        IsBroken = true;
        CombatEventBus.BCOnWeaponBreak();
        SFXManager.i.PlaySFXClip(PlayerSFXList.i.weaponBreak, Game.i.PlayerTransform.position);
    }

    public void SetCurrentAmmo(int amount)
    {
        CurrentAmmo = amount;
    }

    protected void ProcessHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<EnemyBody>(out var body))
        {
            OnNormalHit(); 
            SFXManager.i.PlaySFXClip(UISFXList.i.enemyBodyHit, Game.i.PlayerTransform.position);

            CombatEventBus.BCOnEnemyHit(BaseDamage, false, hit.point);

            body.MyEnemyUnit.TakeDamage(false, BaseDamage);
            Game.i.PlayerUnitInstance.Lifesteal(BaseDamage);
        }
        else if (hit.collider.TryGetComponent<EnemyCritical>(out var enemyCritical))
        {
            OnCriticalHit(); 
            SFXManager.i.PlaySFXClip(UISFXList.i.enemyCritHit, Game.i.PlayerTransform.position);

            int critAdjustedDamage = (int)(BaseDamage * criticalHitModifier);
            Game.i.PlayerUnitInstance.Lifesteal(critAdjustedDamage);

            CombatEventBus.BCOnEnemyHit(critAdjustedDamage, true, hit.point);

            //Debug.Log(enemyCritical.transform.root.GetComponent<EnemyUnit>());
            enemyCritical.MyEnemyUnit.TakeDamage(true, critAdjustedDamage);
        }
    }

    protected virtual void OnCriticalHit() { }
    protected virtual void OnNormalHit() { }
}
