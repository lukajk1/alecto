using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingHeight = 0.2f;
    [SerializeField] private bool usingRigidBody;
    [SerializeField] private bool addRandomStartingOffset;

    private float originalY;
    private float phaseOffset = 0f;
    private float bobStartTime = 0f;
    private bool canBob = false;
    private Rigidbody rb;

    private void Update()
    {
        if (canBob && !usingRigidBody)
            ApplyBobbing();
    }

    private void FixedUpdate()
    {
        if (canBob && usingRigidBody)
            ApplyBobbing();
    }

    public void StartBobbing()
    {
        bobStartTime = Time.time;

        phaseOffset = addRandomStartingOffset
            ? Random.Range(0f, Mathf.PI * 2f)
            : 0f;

        if (usingRigidBody)
            rb = GetComponent<Rigidbody>();

        originalY = transform.position.y;
        canBob = true;
    }

    public void Stop()
    {
        canBob = false;
    }

    private void ApplyBobbing()
    {
        //float elapsed = Time.time - bobStartTime;
        float newY = originalY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        if (usingRigidBody)
        {
            Vector3 targetPosition = new Vector3(rb.position.x, newY, rb.position.z);
            rb.MovePosition(targetPosition);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
