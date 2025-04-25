using UnityEngine;

public class DamageFromKineticEnergy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody collisionRB = collision.gameObject.GetComponent<Rigidbody>();

        if (collisionRB != null)
        {
            float colRelVel = collision.relativeVelocity.magnitude;

            // the majority of the veloctiy must come from the object
            if (collisionRB.linearVelocity.magnitude / colRelVel < 0.5f)
            {
                return;
            }

            // kinetic energy = 1/2 m(v^2)
            float kineticEnergy = 0.5f * collisionRB.mass * colRelVel * colRelVel;

            int damage = (int)(kineticEnergy / 25f);

            if (damage > 5) // avoid small damage--distracting
            {
                Game.i.PlayerUnitInstance.TakeDamage(false, damage);
            }

            //KineticCollisionData colData = new KineticCollisionData(collision, collisionRB, damage, kineticEnergy);
            //colData.PrintInfo();
        }
    }
}
