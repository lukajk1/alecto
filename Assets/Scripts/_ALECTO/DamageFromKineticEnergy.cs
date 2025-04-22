using UnityEngine;

public class DamageFromKineticEnergy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // kinetic energy = 1/2 m(v^2)
            float kineticEnergy = 0.5f * rb.mass * rb.linearVelocity.magnitude * rb.linearVelocity.magnitude;

            // 1kg @ 5m/s = 1 damage--12.5 kg*m^2/s^2 = 12.5 Joules of KE
            int damage = (int)(kineticEnergy / 25f);

            if (damage > 5) // small damage is just annoying
            {
                Game.i.PlayerUnitInstance.TakeDamage(false, damage);
            }
        }
    }
}
