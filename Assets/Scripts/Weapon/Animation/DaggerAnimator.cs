using UnityEngine;

public class DaggerAnimator : WeaponAnimator
{
    public override void SwitchIn()
    {
        base.SwitchIn();
        SFXManager.i.PlaySFXClip(UISFXList.i.daggerSwitchIn, Game.i.PlayerTransform.position);
    }


}
