using UnityEngine;

public class FallDamage : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    PlayerLookAndMove playerLook;
    float speed;
    float speedLastFrame;
    void Start()
    {
        playerLook = FindFirstObjectByType<PlayerLookAndMove>();
    }
    
    private void Update()
    {
        speed = rb.linearVelocity.magnitude;

        if (speed < 1f && speedLastFrame > 15f)
        {
            int damage = (int)(speedLastFrame / 10f);
            if (damage < 3) return;

            Game.i.PlayerUnitInstance.TakeDamage(false, damage);
        }
        speedLastFrame = rb.linearVelocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    { 

    }
}
