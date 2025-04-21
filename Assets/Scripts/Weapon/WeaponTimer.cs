using System;
using System.Collections;
using UnityEngine;

public class WeaponTimer : MonoBehaviour
{
    private Weapon weapon;
    private Coroutine fireCooldown;
    private Coroutine reloadTimer;
    public void Setup(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public bool QueryCanFire()
    {
        if (fireCooldown == null)
        {
            fireCooldown = StartCoroutine(Timer(weapon.FireRate, () => fireCooldown = null));
            //Debug.Log("okay to fire");
            return true;
        }
        else
        {
            //Debug.Log("can't fire");
            return false;
        }
    }

    public bool IsReloading()
    {
        if (reloadTimer == null)
        {
            return false;
        }
        else return true;
    }

    public void Reload(int bulletCountToPutIn)
    {
        if (reloadTimer == null)
        {
            reloadTimer = StartCoroutine(Reload(bulletCountToPutIn, weapon.ReloadSpeed, () => reloadTimer = null));
        }
    }

    private IEnumerator Timer(float duration, Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    private IEnumerator Reload(int bulletCountToPutIn, float duration, Action onComplete) // bad practice to have a second CR probably but also idrc
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();

        weapon.SetCurrentAmmo(weapon.CurrentAmmo + bulletCountToPutIn);
        weapon.TotalAmmo -= bulletCountToPutIn;

        SFXManager.i.PlaySFXClip(UISFXList.i.reloadFinished, Game.i.PlayerTransform.position);
    }
}
