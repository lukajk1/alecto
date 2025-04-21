using System;
using System.Collections;
using UnityEngine;

public class BloodSiphonTimer : MonoBehaviour
{
    private BloodSiphon bloodSiphon;
    private Coroutine regenDelay;
    private Coroutine hasFired;
    private float regenDelayLength;
    public void Setup(BloodSiphon bloodSiphon)
    {
        this.bloodSiphon = bloodSiphon;
        regenDelayLength = bloodSiphon.AmmoRegenRate;
    }

    private void Update()
    {
        if (regenDelay == null && hasFired == null)
        {
            regenDelay = StartCoroutine(Regen(regenDelayLength, () => regenDelay = null));
        }
    }

    private IEnumerator Regen(float duration, Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        if (bloodSiphon.CurrentAmmo < bloodSiphon.ClipSize) bloodSiphon.TotalAmmo--;
        bloodSiphon.SetCurrentAmmo(bloodSiphon.CurrentAmmo + 1);
        onComplete?.Invoke();
    }

    private IEnumerator Timer(float duration, Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    public void HasFired()
    {
        if (hasFired != null)
        {
            StopCoroutine(hasFired);
        }
        hasFired = StartCoroutine(Timer(bloodSiphon.DelayAfterFiringToStartRegen, () => hasFired = null));
    }
}
