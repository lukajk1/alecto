using UnityEngine;

public class CasterBullet : MonoBehaviour
{
    float speed;
    float duration;
    int damage;
    float maxDuration;
    Vector3 dirVector;
    bool go;
    public void Initialize(Vector3 dirVector, float maxDuration, int damage, float bulletSpeed)
    {
        this.dirVector = dirVector;
        this.maxDuration = maxDuration;
        this.damage = damage;
        this.speed = bulletSpeed;
        
        go = true;
    }

    private void Update()
    {
        if (go) duration += Time.deltaTime;
        if (duration >= maxDuration) Destroy(gameObject);

        transform.Translate(dirVector.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.i.PlayerUnitInstance.TakeDamage(false, damage);
        }

        Destroy(gameObject);
    }
}
