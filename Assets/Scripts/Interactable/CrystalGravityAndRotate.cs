using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CrystalGravityAndRotate : MonoBehaviour
{
    private Vector3 rotationSpeed = new Vector3(0, 0, 30f);
    private Rigidbody rb; 
    private float raycastDistance = 2.5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ApplyRandomImpulse();
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance, Color.red);
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
            }
        }
    }
    void ApplyRandomImpulse()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        Vector3 randomDirection = new Vector3(randomX, 0f, randomZ).normalized;

        Vector3 force = randomDirection * 1.6f + Vector3.up * 3f;

        rb.AddForce(force, ForceMode.Impulse);
    }

}
