using System.Collections;
using UnityEngine;

public class EnemyDropExpire : MonoBehaviour
{
    [SerializeField] GameObject parentObject;
    float expireTime = 15f;
    float fiveSecFlashInterval = 0.5f;
    float twoSecFlashInterval = 0.2f;
    float flashOffLength = 0.2f;
    private MeshRenderer meshRenderer;
    void Start()
    {
        StartCoroutine(Expire());
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private IEnumerator Expire() // pretty sure this doesn't work exaclty how I intended but whatever I can fix the timing later if needed
    {
        yield return new WaitForSeconds(expireTime - 7f);

        float elapsed = 0f;
        while (elapsed < 5f)
        {
            yield return new WaitForSeconds(fiveSecFlashInterval);
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(flashOffLength);
            meshRenderer.enabled = true;
            elapsed += fiveSecFlashInterval + flashOffLength;
        }

        elapsed = 0f;
        while (elapsed < 2f)
        {
            yield return new WaitForSeconds(twoSecFlashInterval);
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(flashOffLength);
            meshRenderer.enabled = true;
            elapsed += twoSecFlashInterval + flashOffLength;
        }
        Destroy(parentObject);
    }

}
