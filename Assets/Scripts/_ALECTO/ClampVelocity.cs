using UnityEngine;

public class ClampVelocity : MonoBehaviour
{
    float maxMetersPerSecond = 68f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > maxMetersPerSecond)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxMetersPerSecond;
        }
    }

}
