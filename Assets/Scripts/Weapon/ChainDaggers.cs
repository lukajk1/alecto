using UnityEngine;

public class ChainDaggers : Weapon
{
    public override string Name => "Dagger";
    public override int ClipSize => 7;
    public override int BaseDamage => 60;
    public override float FireRate => 0.50f;
    public override float ReloadSpeed => 1.25f;
    public override float ReadySpeed => 0.35f;
    public override float LifestealRatio => 0.025f;
    public override float Range => 999f;

    public override void Fire(Vector3 firingOrigin, Vector3 forwardFacingVector)
    {
        if (base.TryFire())
        {
            if (Physics.Raycast(firingOrigin, forwardFacingVector, out hit, Range))
            {
                ProcessHit(hit);
            }
        }
    }
    protected override void OnCriticalHit()
    {
        SetCurrentAmmo(CurrentAmmo + 1); // refund ammo on critical hit
    }
    protected override int DecideInitialTotalAmmo()
    {
        return Random.Range(7, 14) * ClipSize;
    }
}
