using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer rend;
    private Material dissolveMaterial;
    [SerializeField] GameObject rootObject;

    private float dissolveTime = 0.85f;
    private float cutoffHeightMax = 3.4f;
    private float cutoffHeightMin = -2.0f;

    public void Begin()
    {
        dissolveMaterial = rend.material;
        StartCoroutine(Dissolve());
    }
    private IEnumerator Dissolve()
    {
        float elapsed = 0f;
        while (elapsed < dissolveTime)
        {
            elapsed += Time.deltaTime;
            dissolveMaterial.SetFloat("_CutoffHeight", Mathf.Lerp(cutoffHeightMax, cutoffHeightMin, elapsed / dissolveTime));
            //if ((elapsed / dissolveTime) >= 0.5f) criticalHitbox.SetActive(false);
            yield return null;
        }
        Destroy(rootObject);
    }
}
