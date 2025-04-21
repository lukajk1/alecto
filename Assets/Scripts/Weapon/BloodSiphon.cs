using UnityEngine;

public class BloodSiphon : Weapon
{
    public override string Name => "Blood Siphon";
    public override int ClipSize => 99;
    public override int BaseDamage => 8;
    public override float FireRate => 0.05f;
    public override float ReloadSpeed => 1f; // this value is not used because this weapon doesn't "reload"
    public override float ReadySpeed => 0.35f;
    public override float LifestealRatio => 0.025f;
    public override float Range => 17f;
    public float AmmoRegenRate => 0.02f;
    public float DelayAfterFiringToStartRegen => 0.2f;

    private BloodSiphonTimer bloodSiphonTimer;

    public BloodSiphon()
    {
        bloodSiphonTimer = new GameObject("BloodSiphonTimer").AddComponent<BloodSiphonTimer>();
        bloodSiphonTimer.Setup(this);
    }

    public override void Fire(Vector3 firingOrigin, Vector3 forwardFacingVector)
    {
        if (base.TryFire())
        {
            bloodSiphonTimer.HasFired();

            if (Physics.Raycast(firingOrigin, forwardFacingVector, out hit, Range))
            {
                ProcessHit(hit);
            }
        }
    }

    public override void Reload()
    {
        // do nothing
    }

}
