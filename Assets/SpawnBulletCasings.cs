using System.Collections;
using UnityEngine;

public class SpawnBulletCasings : MonoBehaviour
{
    public GameObject casing;

    private void OnEnable()
    {
        CombatEventBus.OnWeaponFired += Spawn;
    }
    private void OnDisable()
    {
        CombatEventBus.OnWeaponFired -= Spawn;
    }

    void Spawn(Weapon weapon)
    {
        Transform cam = Camera.main.transform;
        Vector3 right = cam.right;

        Quaternion rotation = cam.rotation * Quaternion.Euler(90f, 0f, 0f);
        GameObject instance = Instantiate(casing, Game.i.PlayerBulletOrigin.position + cam.right * 0.4f, rotation);

        Rigidbody rb = instance.GetComponent<Rigidbody>();

        rb.AddForce((Vector3.up + right) * Random.Range(4f, 7f), ForceMode.Impulse);
        rb.AddTorque(right * Random.Range(-10f, 10f), ForceMode.Impulse);
        
    }
}
