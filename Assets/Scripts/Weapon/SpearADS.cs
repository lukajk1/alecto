using System.Collections;
using UnityEngine;

public class SpearADS : MonoBehaviour
{
    private FOVManager fovManager;
    private float originalFov;
    private PlayerLookAndMove playerLook;
    private float originalSens;

    private float scopedSensRatio = 0.48f;
    private float scopedFOV = 52f;

    private float adsSpeed = 0.08f;
    private bool rightClickDown = false;
    void Start()
    {
        fovManager = FindFirstObjectByType<FOVManager>();
        originalFov = fovManager.FOV;
        playerLook = FindFirstObjectByType<PlayerLookAndMove>();
        originalSens = playerLook.Sensitivity;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!rightClickDown && Inventory.I.GetActiveWeapon() is Spear)
            {
                StartADS();
            }
        }
        else
        {
            if (rightClickDown)
            {
                rightClickDown = false;
                playerLook.Sensitivity = originalSens;
                StartCoroutine(UnADS());
            }
        }

    }
    private void StartADS()
    {
        rightClickDown = true;
        playerLook.Sensitivity = originalSens * scopedSensRatio;
        StartCoroutine(ADS());
    }

    private IEnumerator ADS()
    {
        float elapsed = 0f;
        while (elapsed < adsSpeed)
        {
            elapsed += Time.deltaTime;

            if (!rightClickDown)
            {
                fovManager.FOV = originalFov;
                playerLook.Sensitivity = originalSens;
                StartCoroutine(UnADS());
                yield break;
            }
            
            float t = elapsed / adsSpeed;
            fovManager.FOV = Mathf.Lerp(originalFov, scopedFOV, t);
            yield return null;
        }

        fovManager.FOV = scopedFOV;
    }

    private IEnumerator UnADS()
    {
        float unscopeSpeed = (adsSpeed * fovManager.FOV) / originalFov;

        float elapsed = 0f;
        while (elapsed < unscopeSpeed)
        {
            elapsed += Time.deltaTime;

            if (rightClickDown)
            {
                StartADS();
                yield break;
            }

            float t = elapsed / unscopeSpeed;
            fovManager.FOV = Mathf.Lerp(fovManager.FOV, originalFov, t);

            yield return null;
        }
        yield break;
    }
}
