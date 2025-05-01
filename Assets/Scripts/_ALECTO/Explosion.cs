using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionDecal;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] Light pointLight;

    float sfxMinDist = 8f;
    float sfxMaxDist = 100f;


    private void Start()
    {
        pointLight.enabled = false;
    }
    public void Explode()
    {
        pointLight.enabled = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector3 forceDir = (rb.position - transform.position).normalized;
                rb.AddForce(forceDir * explosionForce, ForceMode.Impulse);

                Vector3 randomTorque = Random.onUnitSphere * explosionForce * 0.5f;
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
        }

        SFXManager.i.PlaySFXClip(EnvSFXList.i.explosion, transform.position, type: SFXManager.SoundType.HasFalloff, minDist: sfxMinDist, maxDist: sfxMaxDist);

        Quaternion rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);
        Instantiate(explosionDecal, transform.position, rotation);
        Instantiate(explosionParticle, transform.position, rotation);

        Destroy(gameObject);
    }
}
