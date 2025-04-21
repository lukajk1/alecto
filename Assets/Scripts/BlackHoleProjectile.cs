using UnityEngine;
using UnityEngine.EventSystems;

public class BlackHoleProjectile : MonoBehaviour
{
    private Vector3 trajectory = Vector3.zero;
    [SerializeField] private float speed = 9f;
    private bool initialized = false;
    private bool jumpImpulseApplied = false;

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private Rigidbody rb;

    public void Initialize(Vector3 trajectory)
    {
        this.trajectory = trajectory.normalized;
        initialized = true;
        jumpImpulseApplied = false;
    }

    private void FixedUpdate()
    {
        if (!initialized) return;

        if (!jumpImpulseApplied)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpImpulseApplied = true;
        }

        rb.AddForce(trajectory * speed, ForceMode.Force);
    }

}
