using System.Collections;
using UnityEngine;

public class SpawnBulletCasings : MonoBehaviour
{
    public GameObject casing;
    public Transform casingOrigin;
    private void OnEnable()
    {
        CombatEventBus.OnWeaponFired += Spawn;
    }
    private void OnDisable()
    {
        CombatEventBus.OnWeaponFired -= Spawn;
    }

    void Spawn(Gun weapon)
    {
        Transform cam = Camera.main.transform;
        Vector3 right = cam.right;

        Quaternion rotation = cam.rotation * Quaternion.Euler(90f, 0f, 0f);
        GameObject instance = Instantiate(casing, casingOrigin.position, rotation);

        Rigidbody rb = instance.GetComponent<Rigidbody>();

        rb.AddForce((Vector3.up + right) * Random.Range(5.5f, 8.5f), ForceMode.Impulse);
        rb.AddTorque(right * Random.Range(-10f, 10f), ForceMode.Impulse);
        
    }
}
