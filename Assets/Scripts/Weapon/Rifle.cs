using UnityEngine;

public class Rifle : Weapon
{
    public override string Name => "AR";
    public override int ClipSize => 65;
    public override int BaseDamage => 22;
    public override float FireRate => 0.10f;
    public override float ReloadSpeed => 1.00f;
    public override float ReadySpeed => 0.35f;
    public override float LifestealRatio => 0.025f;
    public override float Range => 999f;

    public override void Fire(Vector3 firingOrigin, Vector3 forward)
    {
        if (base.TryFire())
        {
            if (Physics.Raycast(firingOrigin, forward, out hit, Range))
            {
                ProcessHit(hit);
            }
            else
            {
                TracerManager.i.FireTracer(forward * 99f);
            }
        }
    }
}
