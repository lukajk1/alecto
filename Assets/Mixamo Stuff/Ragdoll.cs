using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator controller;
    Rigidbody[] rigidbodies;
    private void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdollPhysics(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TurnToRagdoll();
        }
    }
    void TurnToRagdoll()
    {
        controller.enabled = false;
        SetRagdollPhysics(true);
    }

    void SetRagdollPhysics(bool active)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.useGravity = active;
            rb.isKinematic = !active;
        }
    }
}
