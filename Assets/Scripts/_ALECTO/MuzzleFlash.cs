using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class MuzzleFlash : MonoBehaviour
{
    public GameObject muzzleFlash;

    Light pointLight;
    float lightFlashDuration = 0.06f;
    Coroutine lightFlash;
    private void OnEnable()
    {
        CombatEventBus.OnWeaponFired += Fire;
    }

    private void OnDisable()
    {

        CombatEventBus.OnWeaponFired -= Fire;
    }

    void Start()
    {
        pointLight = GetComponent<Light>();
        pointLight.enabled = false;
    }

    void Fire(Weapon weapon)
    {
        Instantiate(muzzleFlash, transform.position, Quaternion.identity, transform);

        if (lightFlash != null)
        {
            StopCoroutine(lightFlash);
            lightFlash = null;
        }
        lightFlash = StartCoroutine(LightFlashCR());
    }

    IEnumerator LightFlashCR()
    {
        pointLight.enabled = true;
        yield return new WaitForSeconds(lightFlashDuration);
        pointLight.enabled = false;
        lightFlash = null;
    }
}
