using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) Explode();
    }
    void Explode()
    {
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
    }
}
