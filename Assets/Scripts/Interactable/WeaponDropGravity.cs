using UnityEngine;

public class WeaponDropGravity : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    private Rigidbody rb;
    private float raycastDistance = 1.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ApplyRandomImpulse();
        ps.transform.SetParent(transform);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance, Color.red);
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Interact")) return;

            if (rb != null)
            {
                //Debug.Log(hit.collider.gameObject.name);

                rb.constraints = RigidbodyConstraints.FreezePosition;
                GetComponent<WeaponDropBobbing>().StartBobbing();

                ps.transform.SetParent(transform.parent); // promote ps transform to sibling so bobbing doesn't offset the ps as well
                ps.Play();
            }
        }
    }

    void ApplyRandomImpulse()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        //Vector3 randomDirection = new Vector3(randomX, 0f, randomZ).normalized;
        Vector3 randomDirection = Game.i.PlayerCamera.transform.forward;

        Vector3 force = randomDirection * 1.6f + Vector3.up * 3f;

        rb.AddForce(force, ForceMode.Impulse);
    }
}
