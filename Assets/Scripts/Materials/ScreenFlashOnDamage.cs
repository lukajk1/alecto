using System.Collections;
using UnityEngine;

public class ScreenFlashOnDamage : MonoBehaviour
{
    [SerializeField] private Material screenFlashMat;
    private float duration = 0.75f;
    float minInterval = 0.5f;
    float maxInterval = 1.88f; // was 1.75 before - a little too subtle
    private void Start()
    {
        screenFlashMat.SetFloat("_VignetteIntensity", minInterval);
    }

    private void OnEnable()
    {
        CombatEventBus.OnPlayerHit += ScreenFlash;
    }

    private void OnDisable()
    {
        CombatEventBus.OnPlayerHit -= ScreenFlash;
    }
    void ScreenFlash(int i, bool b) // have to match event signature
    {
        StartCoroutine(ScreenFlashCR());
    }
    private IEnumerator ScreenFlashCR()
    {
        float halfDuration = duration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            screenFlashMat.SetFloat("_VignetteIntensity", Mathf.Lerp(minInterval, maxInterval, t));
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            screenFlashMat.SetFloat("_VignetteIntensity", Mathf.Lerp(maxInterval, minInterval, t));
            yield return null;
        }

        screenFlashMat.SetFloat("_VignetteIntensity", minInterval); // ensure final value
    }

    public void SetFlashValue(float value)
    {
        screenFlashMat.SetFloat("_VignetteIntensity", value);
    }
}
