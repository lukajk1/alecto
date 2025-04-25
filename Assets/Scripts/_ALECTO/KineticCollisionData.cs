using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public struct KineticCollisionData
{
    public Collision CollisionInstance;
    public Rigidbody RbCollidedWith;
    public int DamageTaken;
    public float KineticEnergyOfImpact;

    public KineticCollisionData(Collision collision, Rigidbody collidedWith, int damageTaken, float kineticEnergyOfImpact)
    {
        CollisionInstance = collision;
        RbCollidedWith = collidedWith;
        DamageTaken = damageTaken;
        KineticEnergyOfImpact = kineticEnergyOfImpact;
    }

    public void PrintInfo()
    {
        Debug.Log($"========");
        Debug.Log($"kinetic energy: {KineticEnergyOfImpact}");
        Debug.Log($"damage taken: {DamageTaken}");
        Debug.Log($"magnitude of relative velocity: {CollisionInstance.relativeVelocity.magnitude}");
        Debug.Log($"mass of object collided with: {RbCollidedWith.mass}");
    }
}
