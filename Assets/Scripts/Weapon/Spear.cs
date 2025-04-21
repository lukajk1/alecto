using UnityEngine;

public class Spear : Weapon
{
    public override string Name => "Spear";
    public override int ClipSize => 7;
    public override int BaseDamage => 230;
    public override float FireRate => 1.55f;
    public override float ReloadSpeed => 1.75f;
    public override float ReadySpeed => 0.7f;
    public override float LifestealRatio => 0.06f;
    public override float Range => 999f;
    protected SpearADS spearADS;
    public Spear()
    {
        spearADS = new GameObject($"SpearADS").AddComponent<SpearADS>();
    }
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

    protected override int DecideInitialTotalAmmo()
    {
        //return Random.Range(2, 4) * ClipSize;
        return Random.Range(0, 1) * ClipSize;
    }
}
