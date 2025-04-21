using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public void AddForceToPlayer(Vector3 force, ForceMode forceMode) 
    {
        rb.AddForce(force, forceMode);
    }
}
